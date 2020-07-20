using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    public class DataTraversalService : IDataTraversalService
    {
        /// <summary>
        /// Traverses a JObject to find the string specified by navigation
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="navigation"></param>
        /// <returns></returns>
        public string Traverse(JObject jObject, string navigation)
        {
            if (jObject is null || string.IsNullOrEmpty(navigation))
                return null;

            var navigationList = new List<string>(navigation.Split("->"));
            return TraverseJObject(jObject, navigationList);
        }

        public string TraverseJSONString(string json, string navigation)
        {
            if (json is null || string.IsNullOrEmpty(navigation))
                return null;

            var jObject = new JObject();
            try
            {
                jObject = JObject.Parse(json);
            }
            catch
            {
                var objString = $"{{'data':{json}}}";
                jObject = JObject.Parse(objString);
                navigation = $"data{navigation}";
            }
            var navigationList = new List<string>(navigation.Split("->"));
            return TraverseJObject(jObject, navigationList);
        }
        /// <summary>
        /// Traverses a HtmlNode to find the string specified by navigation
        /// </summary>
        /// <param name="node"></param>
        /// <param name="navigation"></param>
        /// <returns></returns>
        public string Traverse(HtmlNode node, string navigation)
        {
            if (node is null || string.IsNullOrEmpty(navigation)) 
                return null;

            var navigationList = new List<string>(navigation.Split("->"));
            return TraverseXMLObject(node, navigationList);
        }

        private string TraverseXMLObject(HtmlNode node, List<string> navigationList)
        {
            var nextStep = navigationList[0].ToLower(); //get the next thing to validate to
            if (nextStep.Contains("[")) //arrays are handled differently
                return TraverseXMLArray(node, navigationList);
            else if (navigationList.Count == 1) //we are at the leaf
            {
                if (nextStep.Contains(".")) //check if we're looking at an attribute like img src
                {
                    var leafSplit = nextStep.Split(".");
                    if (leafSplit.Length != 2)
                        return null;
                    return ParseXmlLeaf(node.SelectSingleNode($"./{leafSplit[0]}"), leafSplit[1]); //parse xml leaf grabs the attribute if there is one
                }
                else
                    return ParseXmlLeaf(node.SelectSingleNode($"./{nextStep}"), null); //just grab the text between the tags
            }
            else //we're referencing a subobject. Pop the array and recurse down a level
            {
                navigationList.RemoveAt(0);
                return TraverseXMLObject(node.SelectSingleNode($"./{nextStep}"), navigationList);
            }
        }

        private string TraverseXMLArray(HtmlNode node, List<string> navigationList)
        {
            var nextStep = navigationList[0].ToLower(); 
            bool atLeaf = false; //we need this bool to know whether or not we are checking for attributes or not
            if (navigationList.Count == 1) //we're at the leaf
                atLeaf = true;
            else
                navigationList.RemoveAt(0); //we're not at the leaf just take the first property and work with it
            var arraySplit = new List<string>(nextStep.Split("[")); //split all of the array indexes. 
            var arrayName = arraySplit[0];
            arraySplit.RemoveAt(0);
            var arrayIdxs = new List<int>();
            var leafAttribute = "";
            if(atLeaf) //if we're at leaf then grab the last index and parse out the attribute if we got one. 
            {
                var lastItem = arraySplit[arraySplit.Count - 1];
                if (lastItem.Contains("."))
                {
                    arraySplit.Remove(lastItem);
                    var leafSplit = lastItem.Split(".");
                    if (leafSplit.Length != 2)
                        return null;
                    arraySplit.Add(leafSplit[0]);
                    leafAttribute = leafSplit[1];
                }
            }
            var xmlArray = node.SelectSingleNode($"./{arrayName}"); //get the array object
            foreach (var a in arraySplit)
            {
                var number = a.Replace("]", ""); // get rid of the ]'s
                if (int.TryParse(number, out var idx))
                    arrayIdxs.Add(idx);
            }
            var counter = 1;
            foreach (var idx in arrayIdxs) //go through each index. In the end this array split is describing the numbers in something like html->body[0][3][5][0]. The array would be [0, 3, 5, 0]
            {
                if (counter == arrayIdxs.Count) // at last one
                {
                    if (atLeaf)
                        return ParseXmlLeaf(FindInXmlArray(xmlArray, idx), leafAttribute); //we're at the leaf, parse it
                    else
                        return TraverseXMLObject(FindInXmlArray(xmlArray, idx), navigationList); //get subobject
                }
                else
                    xmlArray = FindInXmlArray(xmlArray, idx); 
                ++counter;
            }
            return null;
        }

        private string ParseXmlLeaf(HtmlNode node, string leafAttribute)
        {
            if (string.IsNullOrEmpty(leafAttribute))
                return node.InnerText;
            else            
                return node.Attributes[leafAttribute].Value;
            
        }

        private HtmlNode FindInXmlArray(HtmlNode node, int idx) //we need this array since random strings in xml and stuff like whitespace gets parsed into nodes with the name "#text" we need to ignore them
        {
            var counter = 0;
            foreach(var child in node.ChildNodes)
            {
                if (child.Name.Equals("#text"))
                    continue;
                else if (counter == idx)
                    return child;
                ++counter;    
            }
            return null;
        }

        private string TraverseJObject(JObject jObject, List<string> navigationList)
        {
            var nextStep = navigationList[0]; //grab next step
            if (nextStep.Contains("[")) //check if this step has array operators, if so then traverse them
                return TraverseJArray(jObject, navigationList);            
            else if (navigationList.Count == 1) //grab the string at the leaf because we're there
                return jObject[navigationList[0]].ToString();
            else //pop and go down a level 
            {
                navigationList.RemoveAt(0); 
                return TraverseJObject((JObject)jObject[nextStep], navigationList);
            }
        }

        private string TraverseJArray(JObject jObject, List<string> navigationList)
        {
            var nextStep = navigationList[0]; 
            bool atLeaf = false; // need this to know if we should be returning a string at the last array operator. 
            if (navigationList.Count == 1)
                atLeaf = true;
            else
                navigationList.RemoveAt(0);
            var arraySplit = new List<string>(nextStep.Split("["));
            var arrayName = arraySplit[0]; //find the name of the array. The nextstep in this method looks something like array1[2][1]. arrayName would be array1
            arraySplit.RemoveAt(0);
            var arrayIdxs = new List<int>();
            var jArray = (JArray)jObject[arrayName];
            foreach (var a in arraySplit)
            {
                var number = a.Replace("]", "");
                if (int.TryParse(number, out var idx)) //need to make sure indexes are actual numbers. 
                    arrayIdxs.Add(idx);
            }
            var counter = 1;
            foreach (var idx in arrayIdxs) //go through each index. In the end this array split is describing the numbers in something like html->body[0][3][5][0]. The array would be [0, 3, 5, 0]
            {
                if (counter == arrayIdxs.Count) // at last one
                {
                    if (atLeaf)
                        return jArray[idx].ToString(); //return string at this index
                    else
                        return TraverseJObject((JObject)jArray[idx], navigationList); //get the subobject at the index
                }
                else
                    jArray = (JArray)jArray[idx]; //grab subarray and keep going down 
                ++counter;
            }
            return null;
        }
    }
}
