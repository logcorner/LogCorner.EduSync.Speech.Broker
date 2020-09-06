using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System;
using System.Collections.Generic;
using Xunit;

namespace LogCorner.EduSync.Speech.ReadModel.UnitTests
{
    public class SpeechViewUnitTest
    {
        [Fact]
        public void ShouldApplySpeechCreatedEvent()
        {
            //Arrange
            var speechView = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            var speechCreatedEvent = new SpeechCreatedEvent(Guid.NewGuid(),
                "my title", "http://test.com", "my desc", "conferences");

            //Act
            speechView.Apply(speechCreatedEvent);

            //Assert
            Assert.Equal(speechCreatedEvent.AggregateId, speechView.Id);
            Assert.Equal(speechCreatedEvent.Title, speechView.Title);
            Assert.Equal(speechCreatedEvent.Description, speechView.Description);
            Assert.Equal(speechCreatedEvent.Url, speechView.Url);
            Assert.Equal(speechCreatedEvent.Type, speechView.Type);
        }

        [Fact]
        public void ShouldApplySpeechTitleChangedEvent()
        {
            //Arrange
            var speechView = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            var speechTitleChangedEvent = new SpeechTitleChangedEvent(Guid.NewGuid(), "my title");

            //Act
            speechView.Apply(speechTitleChangedEvent);

            //Assert
            Assert.Equal(speechTitleChangedEvent.AggregateId, speechView.Id);
            Assert.Equal(speechTitleChangedEvent.Title, speechView.Title);
            Assert.Equal(default, speechView.Description);
            Assert.Equal(default, speechView.Url);
            Assert.Equal(default, speechView.Type);
        }

        [Fact]
        public void ShouldApplySpeechDescriptionChangedEvent()
        {
            //Arrange
            var speechView = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            var speechDescriptionChangedEvent = new SpeechDescriptionChangedEvent(Guid.NewGuid(), "my desc");

            //Act
            speechView.Apply(speechDescriptionChangedEvent);

            //Assert
            Assert.Equal(speechDescriptionChangedEvent.AggregateId, speechView.Id);
            Assert.Equal(speechDescriptionChangedEvent.Description, speechView.Description);
            Assert.Equal(default, speechView.Title);
            Assert.Equal(default, speechView.Url);
            Assert.Equal(default, speechView.Type);
        }

        [Fact]
        public void ShouldApplySpeechUrlChangedEvent()
        {
            //Arrange
            var speechView = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            var speechUrlChangedEvent = new SpeechUrlChangedEvent(Guid.NewGuid(), "my url");
            //Act
            speechView.Apply(speechUrlChangedEvent);

            //Assert
            Assert.Equal(speechUrlChangedEvent.AggregateId, speechView.Id);
            Assert.Equal(speechUrlChangedEvent.Url, speechView.Url);
            Assert.Equal(default, speechView.Title);
            Assert.Equal(default, speechView.Description);
            Assert.Equal(default, speechView.Type);
        }

        [Fact]
        public void ShouldApplySpeechTypeChangedEvent()
        {
            //Arrange
            var speechView = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            var speechTypeChangedEvent = new SpeechTypeChangedEvent(Guid.NewGuid(), "my url");

            //Act
            speechView.Apply(speechTypeChangedEvent);

            //Assert
            Assert.Equal(speechTypeChangedEvent.AggregateId, speechView.Id);
            Assert.Equal(speechTypeChangedEvent.Type, speechView.Type);
            Assert.Equal(default, speechView.Title);
            Assert.Equal(default, speechView.Description);
            Assert.Equal(default, speechView.Url);
        }

        [Fact]
        public void ShouldLoadFromHistory()
        {
            //Arrange
            var id = Guid.NewGuid();
            var speechView = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            var speechCreatedEvent = new SpeechCreatedEvent(id, "my title", "http://test.com", "my desc", "conferences");
            var speechTitleChangedEvent = new SpeechTitleChangedEvent(id, "my title");
            var speechTypeChangedEvent = new SpeechTypeChangedEvent(id, "my url");
            var speechDescriptionChangedEvent = new SpeechDescriptionChangedEvent(id, "my desc");
            var speechUrlChangedEvent = new SpeechUrlChangedEvent(id, "my url");

            //Act
            speechView.LoadFromHistory(new List<IDomainEvent> { speechCreatedEvent, speechTitleChangedEvent, speechDescriptionChangedEvent, speechUrlChangedEvent, speechTypeChangedEvent });

            //Assert
            Assert.Equal(speechCreatedEvent.AggregateId, speechView.Id); Assert.Equal(speechTitleChangedEvent.Title, speechView.Title);
            Assert.Equal(speechDescriptionChangedEvent.Description, speechView.Description);
            Assert.Equal(speechUrlChangedEvent.Url, speechView.Url);
            Assert.Equal(speechTypeChangedEvent.Type, speechView.Type);
        }
    }
}