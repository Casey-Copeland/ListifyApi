using ListifyApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ListifyApi.Tests
{
    public class ListifyControllerTests
    {
        [Fact]
        public void GetAllReturnsAllValuesNoFilter()
        {
            //Given the following setup
            var controller = new ListifyController();
            var expectedValues = Enumerable.Range(0, 100); //Default value assigned is 100

            //When the controller is called
            var values = controller.GetAll(new ListifyFilterModel());
            var OkResult = values as OkObjectResult;

            //Then the response code should match the expected response code
            Assert.Equal(200, OkResult.StatusCode);
            Assert.True(((IEnumerable<int>)OkResult.Value).SequenceEqual(expectedValues));
        }

        [Theory]
        [InlineData(0, 100, 200)]
        [InlineData(100, 100, 400)]
        [InlineData(-25, 100, 200)]
        [InlineData(25, 10, 400)]
        [InlineData(null, 10, 400)]
        [InlineData(25, null, 400)]
        [InlineData(null, null, 200)]
        public void GetAllRequestValidation(int? minValue, int? maxValue, int expectedResponseCode)
        {
            //Given the following setup
            var controller = new ListifyController();

            //When the controller is called
            var response = controller.GetAll(new ListifyFilterModel() { MinValue = minValue, MaxValue = maxValue });
            var statusCode = response as IStatusCodeActionResult;

            //Then the response code should match the expected response code
            Assert.Equal(expectedResponseCode, statusCode.StatusCode);
        }

        [Theory]
        [InlineData(1, 0, 100, 200)]
        [InlineData(10, 10, 100, 200)]
        [InlineData(9, 10, 100, 400)]
        [InlineData(-9, 10, 100, 400)]
        [InlineData(120, 10, 100, 400)]
        [InlineData(10, null, 100, 400)]
        [InlineData(10, 10, null, 400)]
        [InlineData(10, null, null, 200)]
        [InlineData(0, null, null, 200)]
        [InlineData(-25, null, null, 200)]
        public void GetReturnsValue(int index, int? minValue, int? maxValue, int expectedResponseCode)
        {
            //Given the following values
            var controller = new ListifyController();

            //When the controller is called
            var response = controller.Get(index, new ListifyFilterModel() { MinValue = minValue, MaxValue = maxValue });
            var statusCode = response as IStatusCodeActionResult;
            var responseObject = response as OkObjectResult;

            //Then the response code should match the expected response code
            Assert.Equal(expectedResponseCode, statusCode.StatusCode);

            if(statusCode.StatusCode == 200)
                Assert.Equal(minValue.HasValue ? minValue.Value + Math.Abs(index) : index + Math.Abs(index), (int)responseObject.Value);
        }
    }
}
