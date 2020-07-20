using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voicify.Sdk.Webhooks.Services;
using Xunit;

namespace Voicify.Sdk.Webhooks.Data.Unit
{
    public class DataTraversalServiceTests
    {
        private readonly DataTraversalService _dataTraversalService;
        
        public DataTraversalServiceTests()
        {
            _dataTraversalService = new DataTraversalService();
        }

        [Fact]
        public void DataTraversalService_JSONFindsStringBaseLeaf()
        {
            var jObject = JObject.Parse(TEST_JSON);
            _dataTraversalService.Traverse(jObject, "Test1").Should().Be("test1");
        }

        [Fact]
        public void DataTraversalService_ArrayBase()
        {
            _dataTraversalService.TraverseJSONString(TEST_JSON2, "[0]->Test1").Should().Be("test1");
        }

        [Fact]
        public void DataTraversalService_JSONFindsObjectLeaf()
        {
            var jObject = JObject.Parse(TEST_JSON);
            _dataTraversalService.Traverse(jObject, "Test2->Test2Sub->Test2SubSubLarge").Should().Be("33.3333");
            _dataTraversalService.Traverse(jObject, "Test2->Test2Sub->Test2SubSubSmall").Should().Be(".33");
        }

        [Fact]
        public void DataTraversalService_JSONArrayTraversalFindsObjectLeaf()
        {
            var jObject = JObject.Parse(TEST_JSON);
            _dataTraversalService.Traverse(jObject, "TestArray1[0]->TestSubArray1[0]->TestSubArray2[1]->VerifyNestedArrayField").Should().Be("garbage2");
            _dataTraversalService.Traverse(jObject, "TestArray1[1]->TestSubArray1[1][0]->VerifyNestedArrayField").Should().Be("garbage5");
        }

        [Fact]
        public void DataTraversalService_JSONArrayTraversalFindsArrayLeaf()
        {
            var jObject = JObject.Parse(TEST_JSON);
            _dataTraversalService.Traverse(jObject, "TestArray1[0]->TestSubArray1[0]->TestSubArray2[2][1]").Should().Be("field2");
            _dataTraversalService.Traverse(jObject, "TestArray1[1]->TestSubArray1[1][2][3]").Should().Be("field12");
        }

        [Fact]
        public void DataTraversalService_XMLFindsStringBaseLeaf()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(TEST_XML);
            _dataTraversalService.Traverse(doc.DocumentNode, "root->Test1").Should().Be("test1");
            _dataTraversalService.Traverse(doc.DocumentNode, "root->Test1.src").Should().Be("www.google.com");
        }

        [Fact]
        public void DataTraversalService_XMLFindsObjectLeaf()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(TEST_XML);
            _dataTraversalService.Traverse(doc.DocumentNode, "root->Test2->Test2Sub->Test2SubSubLarge").Should().Be("33.3333");
            _dataTraversalService.Traverse(doc.DocumentNode, "root->Test2->Test2Sub->Test2SubSubSmall").Should().Be(".33");
        }

        [Fact]
        public void DataTraversalService_XMLArrayTraversalFindsObjectLeaf()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(TEST_XML);
            _dataTraversalService.Traverse(doc.DocumentNode, "root->TestArray1[0]->TestSubArray1[0]->TestSubArray2[1]->VerifyNestedArrayField").Should().Be("garbage2");
            _dataTraversalService.Traverse(doc.DocumentNode, "root->TestArray1[1]->TestSubArray1[1][0]->VerifyNestedArrayField").Should().Be("garbage5");
        }

        [Fact]
        public void DataTraversalService_XMLArrayTraversalFindsArrayLeaf()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(TEST_XML);
            _dataTraversalService.Traverse(doc.DocumentNode, "root->TestArray1[0]->TestSubArray1[0]->TestSubArray2[2][1]").Should().Be("field2");
            _dataTraversalService.Traverse(doc.DocumentNode, "root->TestArray1[1]->TestSubArray1[1][2][3]").Should().Be("field12");
            _dataTraversalService.Traverse(doc.DocumentNode, "root->TestArray1[1]->TestSubArray1[1][2][3].src").Should().Be("www.google.com");
        }

        private const string TEST_JSON = @"
        {
            'Test1': 'test1',
            'Test2': {
                'Test2Sub': {
                    'Test2SubSubLarge': '33.3333',
                    'Test2SubSubSmall': '.33'
                }
            },
            'TestArray1': [
                {
                    'TestSubArray1': [
                    {
                        'TestSubArray2': [
                            {
                                'VerifyNestedArrayField': 'garbage1'
                            },
                            {
                                'VerifyNestedArrayField': 'garbage2'
                            },
                            ['field1', 'field2', 'field3', 'field4']
                        ]
                    }
                    ]
                },
                {
                    'TestSubArray1': [
                        [
                            {
                                'VerifyNestedArrayField': 'garbage3'
                            },
                            {
                                'VerifySecondNestedArrayField': 'garbage4'
                            },
                            ['field5', 'field6', 'field7', 'field8']
                        ],
                        [
                            {
                                'VerifyNestedArrayField': 'garbage5'
                            },
                            {
                                'VerifySecondNestedArrayField': 'garbage6'
                            },
                            ['field9', 'field10', 'field11', 'field12']
                        ]  
                    ]
                }
            ],
            'TestFieldArray': ['field1', 'field2', 'field3', 'field4']
        }
        ";

        private const string TEST_JSON2 = @"
        [
            {
                'Test1': 'test1',
                'Test2': {
                    'Test2Sub': {
                        'Test2SubSubLarge': '33.3333',
                        'Test2SubSubSmall': '.33'
                    }
                } 
            }
        ]
        ";

        private const string TEST_XML = @"
        <?xml version=""1.0"" encoding=""UTF-8""?>
        <root>
           <Test1 src=""www.google.com"">test1</Test1>
           <Test2>
              <Test2Sub>
                 <Test2SubSubLarge>33.3333</Test2SubSubLarge>
                 <Test2SubSubSmall>.33</Test2SubSubSmall>
              </Test2Sub>
           </Test2>
           <TestArray1>
              <element>
                 <TestSubArray1>
                    <element>
                       <TestSubArray2>
                          <element>
                             <VerifyNestedArrayField>garbage1</VerifyNestedArrayField>
                          </element>
                          <element>
                             <VerifyNestedArrayField>garbage2</VerifyNestedArrayField>
                          </element>
                          <element>
                             <element>field1</element>
                             <element>field2</element>
                             <element>field3</element>
                             <element>field4</element>
                          </element>
                       </TestSubArray2>
                    </element>
                 </TestSubArray1>
              </element>
              <element>
                 <TestSubArray1>
                    <element>
                       <element>
                          <VerifyNestedArrayField>garbage3</VerifyNestedArrayField>
                       </element>
                       <element>
                          <VerifySecondNestedArrayField>garbage4</VerifySecondNestedArrayField>
                       </element>
                       <element>
                          <element>field5</element>
                          <element>field6</element>
                          <element>field7</element>
                          <element>field8</element>
                       </element>
                    </element>
                    <element>
                       <element>
                          <VerifyNestedArrayField>garbage5</VerifyNestedArrayField>
                       </element>
                       <element>
                          <VerifySecondNestedArrayField>garbage6</VerifySecondNestedArrayField>
                       </element>
                       <element>
                          <element>field9</element>
                          <element>field10</element>
                          <element>field11</element>
                          <element src=""www.google.com"">field12</element>
                       </element>
                    </element>
                 </TestSubArray1>
              </element>
           </TestArray1>
           <TestFieldArray>
              <element>field1</element>
              <element>field2</element>
              <element>field3</element>
              <element>field4</element>
           </TestFieldArray>
        </root>
        ";
    }
}
