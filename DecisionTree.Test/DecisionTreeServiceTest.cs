﻿using DecisionTree.Models;
using DecisionTree.Services;
using DecisionTree.Services.Builders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;
using System.Data;

namespace DecisionTree.Test
{
	public class DecisionTreeServiceTest
	{
		[Theory, ]
		[InlineData(
			new string[] { "Outlook", "Wind" },
			new string[] { "sunny", "Strong" }, 
			"Yes")]
		[InlineData(
			new string[] { "Outlook", "Humidity" },
			new string[] { "overcast", "High" },
			"no")]
		public void TestDecisionTree(string[] keys, string[] values, string expectedResult)
		{
			var mock = new Mock<IDecisionTreeBuilder>();
			mock.Setup(x => x.Build(It.IsAny<int[][]>(), It.IsAny<int[]>()))
				.Returns(GetContext());

			var result = new DecisionTreeService(It.IsAny<DataTable>(), It.IsAny<Variable[]>(), mock.Object)
				.GetDecision(CreateContext(keys, values));

			result.Should().BeEquivalentTo(expectedResult);
		}

		private Vector CreateContext(string[] keys, string[] values)
		{
			if (keys.Length != values.Length)
				throw new ArgumentException();

			var line = new Vector();
			for (var i = 0; i < keys.Length; ++i)
			{
				line[keys[i]] = values[i];
			}

			return line;
		}

		private Node GetContext()
		{
			var root = new Node()
			{
				Name = "Outlook"
			};

			var parent1 = new Node() { Name = "Wind" };
			var parent1Sheet = new Node { Name = "Yes" };
			parent1.Childs.Add(new Edge { ParentNode = parent1, ChildNode = parent1Sheet, Value = "Strong" });

			var parent2 = new Node() { Name = "Humidity" };
			var parent2Sheet = new Node { Name = "No" };
			parent2.Childs.Add(new Edge { ParentNode = parent2, ChildNode = parent2Sheet, Value = "High" });

			root.Childs.AddRange(
				new List<Edge>
				{
					new Edge() { ParentNode = root, ChildNode = parent1, Value="sunny"},
					new Edge() { ParentNode = root, ChildNode = parent2, Value="overcast"},
				});

			return root;
		}
	}
}
