﻿namespace MachineLearning.LearnAlgorithms
{
	public interface IDecisionTreeBuilder
	{
		Models.DecisionTree Learn(int[][] inputs, int[] outputs);
	}
}