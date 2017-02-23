﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OrleansNonSiloTesting;
using OrleansUnitTestSilo.Tests.TestGrains;
using Xunit;

namespace OrleansUnitTestSilo.Tests.Tests
{
    public class StorageTests : UnitTestSiloBase
    {
        [Fact]
        public async Task SiloSayHelloArchiveTest()
        {
            long id = new Random().Next();
            const string greeting1 = "Bonjour";
            const string greeting2 = "Hei";

            var grain = Silo.CreateGrain<HelloArchiveGrain>(id);

            // This will directly call the grain under test.
            await grain.SayHello(greeting1);
            await grain.SayHello(greeting2);

            var greetings = (await grain.GetGreetings()).ToList();

            greetings.Should().Contain(greeting1);
            greetings.Should().Contain(greeting2);

            Silo.Storage(grain).Writes.Should().Be(2);
            Silo.State<GreetingArchive>(grain).Greetings.ShouldAllBeEquivalentTo(greetings);
        }
    }
}