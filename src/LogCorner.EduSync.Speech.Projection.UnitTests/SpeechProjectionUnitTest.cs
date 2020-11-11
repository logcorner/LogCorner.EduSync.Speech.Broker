using LogCorner.EduSync.Speech.SharedKernel.Events;
using System;
using System.Collections.Generic;
using Xunit;

namespace LogCorner.EduSync.Speech.Projection.UnitTests
{
    public class SpeechProjectionUnitTest
    {
        [Fact]
        public void ShouldApplySpeechCreatedEvent()
        {
            //Arrange
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechCreatedEvent = new SpeechCreatedEvent(Guid.NewGuid(),
                "my title", "http://test.com", "my desc", new SpeechTypeEnum(1, "conferences"));

            //Act
            speechProjection.Apply(speechCreatedEvent);

            //Assert
            Assert.Equal(speechCreatedEvent.AggregateId, speechProjection.Id);
            Assert.Equal(speechCreatedEvent.Title, speechProjection.Title);
            Assert.Equal(speechCreatedEvent.Description, speechProjection.Description);
            Assert.Equal(speechCreatedEvent.Url, speechProjection.Url);
            Assert.Equal(speechCreatedEvent.Type, speechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechTitleChangedEvent()
        {
            //Arrange
            var SpeechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechTitleChangedEvent = new SpeechTitleChangedEvent(Guid.NewGuid(), "my title");

            //Act
            SpeechProjection.Apply(speechTitleChangedEvent);

            //Assert
            Assert.Equal(speechTitleChangedEvent.AggregateId, SpeechProjection.Id);
            Assert.Equal(speechTitleChangedEvent.Title, SpeechProjection.Title);
            Assert.Equal(default, SpeechProjection.Description);
            Assert.Equal(default, SpeechProjection.Url);
            Assert.Equal(default, SpeechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechDescriptionChangedEvent()
        {
            //Arrange
            var SpeechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechDescriptionChangedEvent = new SpeechDescriptionChangedEvent(Guid.NewGuid(), "my desc");

            //Act
            SpeechProjection.Apply(speechDescriptionChangedEvent);

            //Assert
            Assert.Equal(speechDescriptionChangedEvent.AggregateId, SpeechProjection.Id);
            Assert.Equal(speechDescriptionChangedEvent.Description, SpeechProjection.Description);
            Assert.Equal(default, SpeechProjection.Title);
            Assert.Equal(default, SpeechProjection.Url);
            Assert.Equal(default, SpeechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechUrlChangedEvent()
        {
            //Arrange
            var SpeechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechUrlChangedEvent = new SpeechUrlChangedEvent(Guid.NewGuid(), "my url");
            //Act
            SpeechProjection.Apply(speechUrlChangedEvent);

            //Assert
            Assert.Equal(speechUrlChangedEvent.AggregateId, SpeechProjection.Id);
            Assert.Equal(speechUrlChangedEvent.Url, SpeechProjection.Url);
            Assert.Equal(default, SpeechProjection.Title);
            Assert.Equal(default, SpeechProjection.Description);
            Assert.Equal(default, SpeechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechTypeChangedEvent()
        {
            //Arrange
            var SpeechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechTypeChangedEvent = new SpeechTypeChangedEvent(Guid.NewGuid(), new SpeechTypeEnum(1, "my type"));

            //Act
            SpeechProjection.Apply(speechTypeChangedEvent);

            //Assert
            Assert.Equal(speechTypeChangedEvent.AggregateId, SpeechProjection.Id);
            Assert.Equal(speechTypeChangedEvent.Type, SpeechProjection.Type);
            Assert.Equal(default, SpeechProjection.Title);
            Assert.Equal(default, SpeechProjection.Description);
            Assert.Equal(default, SpeechProjection.Url);
        }

        [Fact]
        public void ShouldLoadFromHistory()
        {
            //Arrange
            var id = Guid.NewGuid();
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechCreatedEvent = new SpeechCreatedEvent(id, "my title", "http://test.com", "my desc", new SpeechTypeEnum(1, "conferences"));
            var speechTitleChangedEvent = new SpeechTitleChangedEvent(id, "my title");
            var speechTypeChangedEvent = new SpeechTypeChangedEvent(id, new SpeechTypeEnum(1, "my title"));
            var speechDescriptionChangedEvent = new SpeechDescriptionChangedEvent(id, "my desc");
            var speechUrlChangedEvent = new SpeechUrlChangedEvent(id, "my url");

            //Act
            speechProjection.LoadFromHistory(new List<IDomainEvent> { speechCreatedEvent, speechTitleChangedEvent, speechDescriptionChangedEvent, speechUrlChangedEvent, speechTypeChangedEvent });

            //Assert
            Assert.Equal(speechCreatedEvent.AggregateId, speechProjection.Id); Assert.Equal(speechTitleChangedEvent.Title, speechProjection.Title);
            Assert.Equal(speechDescriptionChangedEvent.Description, speechProjection.Description);
            Assert.Equal(speechUrlChangedEvent.Url, speechProjection.Url);
            Assert.Equal(speechTypeChangedEvent.Type, speechProjection.Type);
        }
    }
}