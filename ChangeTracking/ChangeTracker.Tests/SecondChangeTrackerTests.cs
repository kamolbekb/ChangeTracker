using _2;
using _2.Models;
using FluentAssertions;
using Xunit;

namespace ChangeTracker.Tests;

public class SecondChangeTrackerTests
{
    [Fact]
    public void PropertyEntry_Should_Track_OriginalValue_And_Not_Modified()
    {
        var person = new Person { Id = 1, FirstName = "John", Age = 30 };
        var propInfo = typeof(Person).GetProperty(nameof(Person.FirstName))!;
        var propEntry = new PropertyEntry(person, propInfo);

        propEntry.PropertyName.Should().Be("FirstName");
        propEntry.OriginalValue.Should().Be("John");
        propEntry.IsModified.Should().BeFalse();
    }

    [Fact]
    public void PropertyEntry_Should_Detect_Change()
    {
        var person = new Person { FirstName = "John" };
        var propInfo = typeof(Person).GetProperty(nameof(Person.FirstName))!;
        var entry = new PropertyEntry(person, propInfo);

        person.FirstName = "Jane";
        entry.CheckIfModified();

        entry.IsModified.Should().BeTrue();
    }

    [Fact]
    public void PropertyEntry_Should_Reset_Modification_On_Same_Value()
    {
        var person = new Person { Age = 25 };
        var propInfo = typeof(Person).GetProperty(nameof(Person.Age))!;
        var entry = new PropertyEntry(person, propInfo);

        entry.CheckIfModified();
        entry.IsModified.Should().BeFalse();
    }

    [Fact]
    public void AcceptChanges_Should_Update_OriginalValue_And_Reset_Modified()
    {
        var person = new Person { FirstName = "John" };
        var propInfo = typeof(Person).GetProperty(nameof(Person.FirstName))!;
        var entry = new PropertyEntry(person, propInfo);

        person.FirstName = "Johnny";
        entry.CheckIfModified();
        entry.IsModified.Should().BeTrue();

        entry.AcceptChanges();

        entry.OriginalValue.Should().Be("Johnny");
        entry.IsModified.Should().BeFalse();
    }

    [Fact]
    public void ChangeTracker_Should_Track_New_Person()
    {
        var tracker = new _2.ChangeTracker();
        var person = new Person { Id = 1, FirstName = "Alice", Age = 22 };

        tracker.Track(person, EntityState.Added);

        var tracked = tracker.GetTrackedEntities();
        tracked.Should().ContainSingle();
        tracked[0].Entity.Should().Be(person);
        tracked[0].State.Should().Be(EntityState.Added);
    }

    [Fact]
    public void ChangeTracker_Should_Update_Tracked_Person_State()
    {
        var tracker = new _2.ChangeTracker();
        var person = new Person { Id = 2, FirstName = "Bob", Age = 35 };

        tracker.Track(person, EntityState.Unchanged);
        tracker.Track(person, EntityState.Deleted);

        var tracked = tracker.GetTrackedEntities();
        tracked.Should().ContainSingle();
        tracked[0].State.Should().Be(EntityState.Deleted);
    }

    [Fact]
    public void ChangeTracker_Should_Clear_All_Entities()
    {
        var tracker = new _2.ChangeTracker();
        tracker.Track(new Person { Id = 3, FirstName = "Charlie", Age = 40 }, EntityState.Added);

        tracker.Clear();

        tracker.GetTrackedEntities().Should().BeEmpty();
    }

    [Fact]
    public void EntityEntry_Should_Detect_Modified_Properties()
    {
        var person = new Person { Id = 4, FirstName = "Dana", Age = 50 };
        var entry = new EntityEntry(person, EntityState.Unchanged);

        person.Age = 51;
        entry.DetectChanges();

        entry.State.Should().Be(EntityState.Modified);
        entry.GetModifiedProperties().Should().ContainSingle(p => p.PropertyName == "Age");
    }

    [Fact]
    public void EntityEntry_Should_Not_Detect_Modifications_If_Unchanged()
    {
        var person = new Person { Id = 5, FirstName = "Ella", Age = 60 };
        var entry = new EntityEntry(person, EntityState.Unchanged);

        entry.DetectChanges();

        entry.State.Should().Be(EntityState.Unchanged);
        entry.GetModifiedProperties().Should().BeEmpty();
    }
}
