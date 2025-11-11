using Bogus;
using FluentAssertions;
using QueryKit.MartenTests.Documents;
using System.Collections.Generic;
using System.Linq;
using Marten.Linq;
using QueryKit.Exceptions;
using Marten;

namespace QueryKit.MartenTests.Tests;

public class MartenGuidFilteringTests : TestBase
{
    [Fact]
    public async Task can_filter_by_guid()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = $"""Id == "{doc1.Id}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc1.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_guid()
    {
        // Arrange
        var faker = new Faker();
        var relatedId = Guid.NewGuid();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = relatedId
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = null
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = $"""RelatedId == "{relatedId}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc1.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_guid_is_null()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = Guid.NewGuid()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = null
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = """RelatedId == null""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc2.Id);
    }

    [Fact(Skip = "Marten does not support ToString() on Guid")]
    public async Task can_filter_by_guid_contains()
    {
        // Arrange
        var faker = new Faker();
        var guidToFind = Guid.NewGuid();
        var doc1 = new TestDocument
        {
            Id = guidToFind,
            Title = faker.Lorem.Sentence()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = $"""Id @= "{guidToFind}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(guidToFind);
    }

    [Fact]
    public async Task can_filter_by_guid_in_operator()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            RelatedId = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            RelatedId = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };
        var doc3 = new TestDocument
        {
            Id = Guid.NewGuid(),
            RelatedId = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1, doc2, doc3);
        await Session.SaveChangesAsync();

        var input = $"""RelatedId ^^ ["{doc1.RelatedId}", "{doc3.RelatedId}"]""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(2);
        results.Should().Contain(x => x.Id == doc1.Id);
        results.Should().NotContain(x => x.Id == doc2.Id);
        results.Should().Contain(x => x.Id == doc3.Id);
    }

    [Fact]
    public async Task can_filter_by_additionalids_has_operator()
    {
        // Arrange
        var guidToFind = Guid.NewGuid();
        var docWithGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Has Guid",
            AdditionalIds = new[] { guidToFind, Guid.NewGuid() }
        };
        var docWithoutGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "No Guid",
            AdditionalIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
        };

        Session.Store(docWithGuid, docWithoutGuid);
        await Session.SaveChangesAsync();

        var input = $"""AdditionalIds ^$ {guidToFind} """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithGuid.Id);
    }

    [Fact]
    public async Task can_filter_by_additionalids_does_not_have_operator()
    {
        // Arrange
        var guidToExclude = Guid.NewGuid();
        var docWithGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Has Guid",
            AdditionalIds = new[] { guidToExclude, Guid.NewGuid() }
        };
        var docWithoutGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "No Guid",
            AdditionalIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
        };

        Session.Store(docWithGuid, docWithoutGuid);
        await Session.SaveChangesAsync();

        var input = $"""AdditionalIds !^$ "{guidToExclude}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithoutGuid.Id);
    }

    [Fact]
    public async Task can_filter_by_nullableadditionalids_has_operator()
    {
        // Arrange
        var guidToFind = Guid.NewGuid();
        var docWithGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Has Guid",
            NullableAdditionalIds = new[] { guidToFind, Guid.NewGuid() }
        };
        var docWithoutGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "No Guid",
            NullableAdditionalIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
        };

        Session.Store(docWithGuid, docWithoutGuid);
        await Session.SaveChangesAsync();

        var input = $"""NullableAdditionalIds ^$ {guidToFind} """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithGuid.Id);
    }

    [Fact]
    public async Task can_filter_by_nullableadditionalids_does_not_have_operator()
    {
        // Arrange
        var guidToExclude = Guid.NewGuid();
        var docWithGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Has Guid",
            NullableAdditionalIds = new[] { guidToExclude, Guid.NewGuid() }
        };
        var docWithoutGuid = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "No Guid",
            NullableAdditionalIds = new[] { Guid.NewGuid(), Guid.NewGuid() }
        };

        Session.Store(docWithGuid, docWithoutGuid);
        await Session.SaveChangesAsync();

        var input = $"""NullableAdditionalIds !^$ "{guidToExclude}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithoutGuid.Id);
    }

    [Fact]
    public async Task can_filter_with_complex_conditions()
    {
        // Arrange
        var faker = new Faker();
        var specificDateTime = new DateTimeOffset(2022, 7, 1, 0, 0, 3, TimeSpan.Zero);
        var specificDate = new DateOnly(2022, 7, 1);
        var specificTime = new TimeOnly(0, 0, 3);

        // Document that should match via Rating > 3.5
        var matchingDoc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "waffle & chicken special",
            Age = 35,
            Rating = 4.0M,  // This will match via Rating > 3.5
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = null,
            Date = null,
            Time = null
        };

        // Document that should match via GUID condition AND Age < 18
        var matchingDoc2 = new TestDocument
        {
            Id = Guid.Parse("aa648248-cb69-4217-ac95-d7484795afb2"),
            Title = "something else",
            Age = 15,  // Changed to satisfy Age < 18
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = null,
            Date = null,
            Time = null
        };

        // Document that should match via Title == "lamb" AND Age < 18
        var matchingDoc3 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "lamb",
            Age = 15,  // Changed to satisfy Age < 18
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = null,
            Date = null,
            Time = null
        };

        // Document that should match via Title == null AND Age < 18
        var matchingDoc4 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = null,
            Age = 15,
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = null,
            Date = null,
            Time = null
        };

        // Document that should match via Title contains "waffle & chicken" AND Age > 30 AND BirthMonth == January AND Title starts with "ally"
        var matchingDoc6 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "ally smith with waffle & chicken",  // Now matches both Title _= "ally" and Title @=* "waffle & chicken"
            Age = 35,  // Now matches Age > 30
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.January,
            SpecificDate = null,
            Date = null,
            Time = null
        };

        // Document that should match via dates
        var matchingDoc8 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "something else",
            Age = 25,
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = specificDateTime,
            Date = specificDate,
            Time = null
        };

        // Document that should match via dates (time condition)
        var matchingDoc9 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "something else",
            Age = 25,
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = specificDateTime,
            Date = null,
            Time = specificTime
        };

        // Document that should not match any conditions
        var nonMatchingDoc = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "no match",
            Age = 25,
            Rating = 2.5M,
            BirthMonth = BirthMonthEnum.February,
            SpecificDate = DateTimeOffset.Now,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Time = TimeOnly.FromDateTime(DateTime.Now)
        };

        Session.Store(matchingDoc1, matchingDoc2, matchingDoc3, matchingDoc4,
            matchingDoc6, matchingDoc8, matchingDoc9, nonMatchingDoc);
        await Session.SaveChangesAsync();

        var input = """((Title @=* "waffle & chicken" && Age > 30) || Id == "aa648248-cb69-4217-ac95-d7484795afb2" || Title == "lamb" || Title == null) && (Age < 18 || (BirthMonth == January && Title _= "ally")) || Rating > 3.5 || SpecificDate == 2022-07-01T00:00:03Z && (Date == 2022-07-01 || Time == 00:00:03)""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Should().Contain(x => x.Id == matchingDoc1.Id);
        results.Should().Contain(x => x.Id == matchingDoc2.Id);
        results.Should().Contain(x => x.Id == matchingDoc3.Id);
        results.Should().Contain(x => x.Id == matchingDoc4.Id);
        results.Should().Contain(x => x.Id == matchingDoc6.Id);
        results.Should().Contain(x => x.Id == matchingDoc8.Id);
        results.Should().Contain(x => x.Id == matchingDoc9.Id);
        results.Should().NotContain(x => x.Id == nonMatchingDoc.Id);

        // Verify specific matching conditions
        var doc1 = results.FirstOrDefault(x => x.Id == matchingDoc1.Id);
        doc1.Should().NotBeNull("matches Rating > 3.5");

        var doc2 = results.FirstOrDefault(x => x.Id == matchingDoc2.Id);
        doc2.Should().NotBeNull("matches specific GUID AND Age < 18");

        var doc3 = results.FirstOrDefault(x => x.Id == matchingDoc3.Id);
        doc3.Should().NotBeNull("matches Title == lamb AND Age < 18");

        var doc4 = results.FirstOrDefault(x => x.Id == matchingDoc4.Id);
        doc4.Should().NotBeNull("matches 'Title is null' AND 'Age < 18'");

        var doc6 = results.FirstOrDefault(x => x.Id == matchingDoc6.Id);
        doc6.Should().NotBeNull("matches 'Title contains waffle & chicken AND Age > 30' AND 'BirthMonth == January AND Title starts with ally'");

        var doc8 = results.FirstOrDefault(x => x.Id == matchingDoc8.Id);
        doc8.Should().NotBeNull("matches SpecificDate AND Date conditions");

        var doc9 = results.FirstOrDefault(x => x.Id == matchingDoc9.Id);
        doc9.Should().NotBeNull("matches SpecificDate AND Time conditions");

        results.FirstOrDefault(x => x.Id == nonMatchingDoc.Id).Should().BeNull("doesn't match any conditions");
    }

    [Fact]
    public async Task can_filter_by_name_and_additionalids_with_or_on_guid_array()
    {
        // Arrange
        var testPrefix = "prefix-" + Guid.NewGuid().ToString("N").Substring(0, 8);
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        var id4 = Guid.NewGuid();

        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = testPrefix + "-one",
            AdditionalIds = new[] { id1, id2 }
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = testPrefix + "-two",
            AdditionalIds = new[] { id3 }
        };
        var doc3 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = testPrefix + "-three",
            AdditionalIds = new[] { id4 }
        };
        var doc4 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "other-title",
            AdditionalIds = new[] { id2, id3 }
        };

        Session.Store(doc1, doc2, doc3, doc4);
        await Session.SaveChangesAsync();

        var input = $"""Title @= "{testPrefix}" && (AdditionalIds ^$ {id2} || AdditionalIds ^$ "{id3}")""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Should().ContainSingle(x => x.Id == doc1.Id);
        results.Should().ContainSingle(x => x.Id == doc2.Id);
        results.Should().NotContain(x => x.Id == doc3.Id);
        results.Should().NotContain(x => x.Id == doc4.Id);
    }

    [Fact]
    public async Task should_handle_malformed_guid_gracefully()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1);
        await Session.SaveChangesAsync();

        var input = """Id == "not-a-guid" """;

        // Act & Assert
        var queryable = Session.Query<TestDocument>();
        var exception = await Assert.ThrowsAsync<ParsingException>(() =>
            queryable.ApplyQueryKitFilter(input).ToListAsync());
        exception.Message.Should().Contain("parsing failure");
    }

    [Fact]
    public async Task should_handle_invalid_guid_in_array()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1);
        await Session.SaveChangesAsync();

        var input = """Id ^^ ["not-a-guid", "also-not-a-guid"]""";

        // Act & Assert
        var queryable = Session.Query<TestDocument>();
        var exception = await Assert.ThrowsAsync<ParsingException>(() =>
            queryable.ApplyQueryKitFilter(input).ToListAsync());
        exception.Message.Should().Contain("parsing failure");
    }

    [Fact]
    public async Task should_handle_empty_guid_string()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1);
        await Session.SaveChangesAsync();

        var input = """Id == "" """;

        // Act & Assert
        var queryable = Session.Query<TestDocument>();
        var exception = await Assert.ThrowsAsync<ParsingException>(() =>
            queryable.ApplyQueryKitFilter(input).ToListAsync());
        exception.Message.Should().Contain("parsing failure");
    }

    [Fact]
    public async Task should_handle_non_guid_string()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1);
        await Session.SaveChangesAsync();

        var input = """Id == "123" """;

        // Act & Assert
        var queryable = Session.Query<TestDocument>();
        var exception = await Assert.ThrowsAsync<ParsingException>(() =>
            queryable.ApplyQueryKitFilter(input).ToListAsync());
        exception.Message.Should().Contain("parsing failure");
    }
}