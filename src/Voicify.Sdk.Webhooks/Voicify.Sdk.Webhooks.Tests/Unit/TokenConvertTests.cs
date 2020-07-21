using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voicify.Sdk.Webhooks.Services;
using Xunit;

namespace Voicify.Sdk.Webhooks.Data.Unit
{
    public class TokenConvertTests
    {
        private class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        [Fact]
        public void SuccessfullyEncryptsAndDecrypts()
        {
            var testObj = new Credentials
            {
                Username = "test1",
                Password = "test2"
            };
            var key = Guid.NewGuid().ToString();

            var token = TokenConvert.SerializeEncryptedToken(testObj, key, "123123salt 1");

            var newObj = TokenConvert.DeserializeEncryptedToken<Credentials>(token, key, "123123salt 1");

            newObj.Username.Should().Be("test1");
            newObj.Password.Should().Be("test2");
        }

        [Fact]
        public void FailsWithDifferentKeys()
        {
            var testObj = new Credentials
            {
                Username = "test1",
                Password = "test2"
            };
            var key = Guid.NewGuid().ToString();

            var token = TokenConvert.SerializeEncryptedToken(testObj, key, "123123salt 1");
            var success = true;
            try
            {
                var newObj = TokenConvert.DeserializeEncryptedToken<Credentials>(token, "DSGSDGGSDGS", "123123salt 1");
            }
            catch
            {
                success = false;
            }

            success.Should().BeFalse();
        }

        [Fact]
        public void FailsWithDifferentSalts()
        {
            var testObj = new Credentials
            {
                Username = "test1",
                Password = "test2"
            };
            var key = Guid.NewGuid().ToString();

            var token = TokenConvert.SerializeEncryptedToken(testObj, key, "123123salt 1");
            var success = true;
            try
            {
                var newObj = TokenConvert.DeserializeEncryptedToken<Credentials>(token, key, "123123salt 2");
            }
            catch
            {
                success = false;
            }

            success.Should().BeFalse();
        }
    }
}
