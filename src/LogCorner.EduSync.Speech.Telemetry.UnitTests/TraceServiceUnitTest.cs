using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace LogCorner.EduSync.Speech.Telemetry.UnitTests
{
    public class TraceServiceUnitTest
    {
        #region Producer Activity

        [Fact]
        public void StartProducerActivityWithNullActivitySourceNameShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(string.Empty);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            //Assert

            Assert.Throws<ArgumentNullException>(() => traceService.StartActivity(It.IsAny<string>()));
        }

        [Fact]
        public void StartProducerActivityWithNullActivityNameShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            //Assert

            Assert.Throws<ArgumentNullException>(() => traceService.StartActivity(null));
        }

        [Fact]
        public void ShouldStartProducerActivity()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var activityListener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
            };

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            string activityName = "TestActivity";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act

            ActivitySource.AddActivityListener(activityListener);
            var activity = traceService.StartActivity(activityName);

            //Assert
            Assert.NotNull(activity);
            Assert.Equal(activityName, activity.DisplayName);
            Assert.Equal(activityName, activity.OperationName);
            Assert.Equal(openTelemetrySourceName, activity.Source.Name);
            Assert.Equal(ActivityStatusCode.Unset, activity.Status);
            Assert.Equal(ActivityKind.Producer, activity.Kind);
        }

        #endregion Producer Activity

        #region Consumer Activity

        [Fact]
        public void StartConsumerActivityWithNullActivitySourceNameShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(string.Empty);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            //Assert

            Assert.Throws<ArgumentNullException>(() => traceService.StartActivity(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));
        }

        [Fact]
        public void StartConsumerActivityWithNullActivityNameShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            //Assert

            Assert.Throws<ArgumentNullException>(() => traceService.StartActivity(null, It.IsAny<IDictionary<string, object>>()));
        }

        [Fact]
        public void ShouldStartConsumerActivity()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var activityListener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
            };

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            string activityName = "TestActivity";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act

            ActivitySource.AddActivityListener(activityListener);
            var activity = traceService.StartActivity(activityName, It.IsAny<IDictionary<string, object>>());

            //Assert
            Assert.NotNull(activity);
            Assert.Equal(activityName, activity.DisplayName);
            Assert.Equal(activityName, activity.OperationName);
            Assert.Equal(openTelemetrySourceName, activity.Source.Name);
            Assert.Equal(ActivityStatusCode.Unset, activity.Status);
            Assert.Equal(ActivityKind.Consumer, activity.Kind);
        }

        /*[Fact]
        public void ShouldExtractTraceContextFromHeader()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var activityListener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
            };

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            string activityName = "TestActivity";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act

            ActivitySource.AddActivityListener(activityListener);
            var activity = traceService.StartActivity(activityName, It.IsAny<IDictionary<string, object>>());

            //Assert
            Assert.NotNull(activity);
            Assert.Equal(activityName, activity.DisplayName);
            Assert.Equal(activityName, activity.OperationName);
            Assert.Equal(openTelemetrySourceName, activity.Source.Name);
            Assert.Equal(ActivityStatusCode.Unset, activity.Status);
            Assert.Equal(ActivityKind.Consumer, activity.Kind);
        }*/

        #endregion Consumer Activity

        [Fact]
        public void AddActivityTagsWithNullTagsShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            //Assert

            Assert.Throws<ArgumentNullException>(() => traceService.AddActivityTags(It.IsAny<Activity>(), null));
        }

        [Fact]
        public void AddActivityTagsWithNullActivityShouldNotRaiseAnyException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            IDictionary<string, object> tags = new Dictionary<string, object>
            {
                {"messaging.system", "kafka"},
                {"messaging.destination_kind", "queue"},
                {"messaging.kafka.queue", "sample"}
            };
            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            var exception = Record.Exception(() => traceService.AddActivityTags(It.IsAny<Activity>(), tags));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public void ShouldAddActivityTags()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var activityListener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
            };

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            string activityName = "TestActivity";
            IDictionary<string, object> tags = new Dictionary<string, object>
            {
                {"messaging.system", "kafka"},
                {"messaging.destination_kind", "queue"},
                {"messaging.kafka.queue", "sample"}
            };
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);
            ActivitySource.AddActivityListener(activityListener);
            var activity = traceService.StartActivity(activityName);

            //Act
            traceService.AddActivityTags(activity, tags);

            //Assert
            Assert.NotNull(activity.Tags);
            var activityTags = activity.Tags.ToDictionary(x => x.Key, x => x.Value);
            var dicoTags = tags.ToDictionary(x => x.Key, x => (string)x.Value);
            Assert.Equal(dicoTags, activityTags);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddActivityEventWithNullEventNameShouldRaiseArgumentNullException(string eventName)
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => traceService.AddActivityEvent(It.IsAny<Activity>(), eventName, It.IsAny<Dictionary<string, object>>()));
        }

        [Fact]
        public void AddActivityEventWithNullActivityShouldNotRaiseAnyException()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);
            string eventName = "myEvent";
            ITraceService traceService = new TraceService(mockConfiguration.Object);

            //Act
            var exception = Record.Exception(() => traceService.AddActivityEvent(It.IsAny<Activity>(), eventName, It.IsAny<Dictionary<string, object>>()));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public void ShouldAddActivityEvents()
        {
            //Arrange
            var mockConfiguration = new Mock<IConfiguration>();

            var activityListener = new ActivityListener
            {
                ShouldListenTo = _ => true,
                SampleUsingParentId = (ref ActivityCreationOptions<string> _) => ActivitySamplingResult.AllData,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData
            };

            var openTelemetrySourceName = "TestOpenTelemetrySourceName";
            string activityName = "TestActivity";
            IDictionary<string, object> tags = new Dictionary<string, object>
            {
                {"messaging.system", "kafka"},
                {"messaging.destination_kind", "queue"},
                {"messaging.kafka.queue", "sample"}
            };
            var eventName = "myEvent";
            mockConfiguration.Setup(c => c["OpenTelemetry:SourceName"]).Returns(openTelemetrySourceName);

            ITraceService traceService = new TraceService(mockConfiguration.Object);
            ActivitySource.AddActivityListener(activityListener);
            var activity = traceService.StartActivity(activityName);

            //Act

            traceService.AddActivityEvent(activity, eventName, tags);

            //Assert
            Assert.NotNull(activity.Tags);
            var activityTags = activity.Events.SingleOrDefault().Tags.ToDictionary(x => x.Key, x => x.Value);
            var dicoTags = tags.ToDictionary(x => x.Key, x => x.Value);
            Assert.Equal(eventName, activity.Events.SingleOrDefault().Name);

            Assert.Equal(dicoTags, activityTags);
        }
    }
}