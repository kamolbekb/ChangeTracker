using Xunit;
using _1;
using FluentAssertions;

namespace ChangeTracker.Tests;

public class FirstChangeTrackerTests
{
    private class DummyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DummyEntity other && Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }

    [Fact]
    public void Track_AddsNewEntity_WhenNotAlreadyTracked()
    {
        var tracker = new _1.ChangeTracker();
        var entity = new DummyEntity { Id = 1, Name = "Test" };

        tracker.Track(entity, EntityState.Added);

        var tracked = tracker.GetTrackedEntities();
        tracked.Should().HaveCount(1);
        tracked[0].Entity.Should().Be(entity);
        tracked[0].State.Should().Be(EntityState.Added);
    }

    [Fact]
    public void Track_UpdatesState_WhenEntityIsAlreadyTracked()
    {
        var tracker = new _1.ChangeTracker();
        var entity = new DummyEntity { Id = 1, Name = "Test" };

        tracker.Track(entity, EntityState.Added);
        tracker.Track(entity, EntityState.Modified);

        var tracked = tracker.GetTrackedEntities();
        tracked.Should().HaveCount(1);
        tracked[0].State.Should().Be(EntityState.Modified);
    }

    [Fact]
    public void GetTrackedEntities_ReturnsAllEntities()
    {
        var tracker = new _1.ChangeTracker();
        tracker.Track(new DummyEntity { Id = 1 }, EntityState.Added);
        tracker.Track(new DummyEntity { Id = 2 }, EntityState.Deleted);

        var tracked = tracker.GetTrackedEntities();

        tracked.Should().HaveCount(2);
    }

    [Fact]
    public void Clear_RemovesAllTrackedEntities()
    {
        var tracker = new _1.ChangeTracker();
        tracker.Track(new DummyEntity { Id = 1 }, EntityState.Added);

        tracker.Clear();

        var tracked = tracker.GetTrackedEntities();
        tracked.Should().BeEmpty();
    }
}
