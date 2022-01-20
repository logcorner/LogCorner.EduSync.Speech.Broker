using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
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
            Assert.Equal(0, speechProjection.Version);
        }

        [Fact]
        public void ShouldApplySpeechTitleChangedEvent()
        {
            //Arrange
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechTitleChangedEvent = new SpeechTitleChangedEvent(Guid.NewGuid(), "my title");

            //Act
            speechProjection.Apply(speechTitleChangedEvent);

            //Assert
            Assert.Equal(speechTitleChangedEvent.AggregateId, speechProjection.Id);
            Assert.Equal(speechTitleChangedEvent.Title, speechProjection.Title);
            Assert.Equal(default, speechProjection.Description);
            Assert.Equal(default, speechProjection.Url);
            Assert.Equal(default, speechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechDescriptionChangedEvent()
        {
            //Arrange
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechDescriptionChangedEvent = new SpeechDescriptionChangedEvent(Guid.NewGuid(), "my desc");

            //Act
            speechProjection.Apply(speechDescriptionChangedEvent);

            //Assert
            Assert.Equal(speechDescriptionChangedEvent.AggregateId, speechProjection.Id);
            Assert.Equal(speechDescriptionChangedEvent.Description, speechProjection.Description);
            Assert.Equal(default, speechProjection.Title);
            Assert.Equal(default, speechProjection.Url);
            Assert.Equal(default, speechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechUrlChangedEvent()
        {
            //Arrange
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechUrlChangedEvent = new SpeechUrlChangedEvent(Guid.NewGuid(), "my url");
            //Act
            speechProjection.Apply(speechUrlChangedEvent);

            //Assert
            Assert.Equal(speechUrlChangedEvent.AggregateId, speechProjection.Id);
            Assert.Equal(speechUrlChangedEvent.Url, speechProjection.Url);
            Assert.Equal(default, speechProjection.Title);
            Assert.Equal(default, speechProjection.Description);
            Assert.Equal(default, speechProjection.Type);
        }

        [Fact]
        public void ShouldApplySpeechTypeChangedEvent()
        {
            //Arrange
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechTypeChangedEvent = new SpeechTypeChangedEvent(Guid.NewGuid(), new SpeechTypeEnum(1, "my type"));

            //Act
            speechProjection.Apply(speechTypeChangedEvent);

            //Assert
            Assert.Equal(speechTypeChangedEvent.AggregateId, speechProjection.Id);
            Assert.Equal(speechTypeChangedEvent.Type, speechProjection.Type);
            Assert.Equal(default, speechProjection.Title);
            Assert.Equal(default, speechProjection.Description);
            Assert.Equal(default, speechProjection.Url);
        }

        [Fact]
        public void ShouldApplySpeechDeletedEvent()
        {
            //Arrange
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechTypeChangedEvent = new SpeechDeletedEvent(Guid.NewGuid(), true);

            //Act
            speechProjection.Apply(speechTypeChangedEvent);

            //Assert
            Assert.Equal(speechTypeChangedEvent.AggregateId, speechProjection.Id);
            Assert.True(speechProjection.IsDeleted);
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

        [Fact]
        public void ShouldProjectEvent()
        {
            //Arrange
            var id = Guid.NewGuid();
            var speechProjection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            var speechCreatedEvent = new SpeechCreatedEvent(id, "my title", "http://test.com", "my desc", new SpeechTypeEnum(1, "conferences"));
            var speechTitleChangedEvent = new SpeechTitleChangedEvent(id, "my title");

            //Act
            speechProjection.Project(speechCreatedEvent);

            //Assert
            Assert.Equal(speechCreatedEvent.AggregateId, speechProjection.Id); Assert.Equal(speechTitleChangedEvent.Title, speechProjection.Title);
        }
    }
}