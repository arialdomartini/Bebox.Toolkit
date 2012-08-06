using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;
using System.Linq;

namespace Bebox.Toolkit.Tests.Integration
{
	[TestFixture]
	public class ConfigurationHandlerTest
	{
		[Test]
		public void Should_be_able_to_deserialize_a_sample_class()
		{
			var sampleParentClass = System.Configuration.ConfigurationSettings.GetConfig("samplesection") as SampleParentClass;

			sampleParentClass.Name.Should().Be.EqualTo("John");
			sampleParentClass.Surname.Should().Be.EqualTo("Lumbs");

			sampleParentClass.Children.Count.Should().Be.EqualTo(2);
			sampleParentClass.Children.SingleOrDefault(c => c.Value == 1).Should().Not.Be.Null();
			sampleParentClass.Children.SingleOrDefault(c => c.Value == 2).Should().Not.Be.Null();
		}
	}

	public class SampleConfigurationHandler : ConfigurationHandler<SampleParentClass> { }

	public class SampleParentClass
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public List<SampleChild> Children { get; set; }
	}

	public class SampleChild
	{
		public int Value { get; set; }
	}
}