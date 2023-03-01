using System;
using System.Collections.Generic;
using Accord.MachineLearning;
using Accord.Math;
using Accord.Math.Distances;
using Accord.MachineLearning.Clustering;
using UnityEngine;
using Accord.Statistics ; 

public class BestKmeans : MonoBehaviour
{
	public int MinK = 2; // Minimum number of clusters to consider
	int nbKinectMax = 5; // Maximum number of clusters to consider
	public int MaxIterations = 100; // Maximum number of iterations for the KMeans algorithm

	 // List of data points
	//Pour récuperer les données clients
	DataCollecter collector;
	public KinectWrapper.NuiSkeletonFrame[] calibratedClientsSkeletonData;
	List<int> clients = new List<int>();
	double[][] data = new double[10][];
	double[] weights = new double[10];
	int datacount = 0 ;

	// Use this for initialization
	void Start()
	{
		collector = GetComponent<DataCollecter>();
		nbKinectMax = collector.GetnbKinectMax();
		
		// Poids égaux pour chaque point de données

		// Generate some random data points




//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	}
	public void DataTransformation (KinectWrapper.NuiSkeletonFrame[] calibratedClientsSkeletonData , int[]  clients , int j )
	{
		KinectWrapper.NuiSkeletonData SkeletonData = new KinectWrapper.NuiSkeletonData();
		SkeletonData.SkeletonPositions = new UnityEngine.Vector4[20];
		for (int f = 0; f < clients.Length; f++) {
			data[f] = new double[] {  calibratedClientsSkeletonData [clients [f]].SkeletonData [0].SkeletonPositions [j].x,  calibratedClientsSkeletonData [clients [f]].SkeletonData [0].SkeletonPositions [j].y,  calibratedClientsSkeletonData [clients [f]].SkeletonData [0].SkeletonPositions [j].z };
			datacount = datacount + 1;
		}
		for (int i = 0; i < weights.Length; i++)
		{
			weights[i] = 1.0;
		}
		
	} 
	public double[] KmeansAlgo(KinectWrapper.NuiSkeletonFrame[] calibratedClientsSkeletonData , int[]  clients , int j) 
	{
		// Find the optimal number of clusters using the Silhouette algorithm
			int bestK = 0;
		double bestScore = double.NegativeInfinity;
		DataTransformation (calibratedClientsSkeletonData, clients, j);


		for (int k = MinK; k <= nbKinectMax; k++)
		{
			var kMeans = new KMeans(k)
			{
				Distance = new Euclidean(),
				Tolerance = 0.0001,
				MaxIterations = MaxIterations
			};

			// Application de l'algorithme K-Means
			var clusters = kMeans.Learn(data,weights );
			var labels = kMeans.Clusters.Decide(data, weights);
			// Calcul du score de silhouette

			//var silhouette = new Silhouette(points.ToArray(), labels, new Euclidean()).Score;
			var silhouette = ComputeSilhouette ( labels, data , datacount);
			Console.WriteLine("K = " + k + ", Silhouette score = " + silhouette);

			if (silhouette > bestScore)
			{
				bestScore = silhouette;
				bestK = k;
			}
		}
		Console.WriteLine("bestK = " + bestK+ ", bestScore = " + bestScore);
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// Create the KMeans clustering model with the optimal number of clusters
		var bestKMeans = new KMeans(bestK)
		{
			Distance = new Euclidean(),
			Tolerance = 0.0001,
			MaxIterations = MaxIterations
		};

		// Fit the model to the data points
		var bestClusters = bestKMeans.Learn(data,weights);

		// Get the cluster labels for each data point
		var bestLabels = bestKMeans.Clusters.Decide(data,weights);
		return  bestLabels;

	}


	public double ComputeSilhouette (double[] labels, double[][] data , int datacount)

	{
		int n = labels.Length; 
		double SilhouetteScore = 0 ;
		double cptlabel = 0.0;
		int c = 0;
		var Distance = new Euclidean (); 

		while( labels[c] !=labels[n] ){
			cptlabel = cptlabel + 1.0;
			c = c + 1;
		}


		for (int i = 0; i < datacount; i++)
		{
			double ai = 0;
			double bi = double.MaxValue;
			int cpta = 0;
		


			for (int j = 0; j < datacount; j++)
			{
				if (labels [i] == labels [j] && i != j)
				{
					ai +=  Distance.Distance (data[i], data[j]);
					cpta = cpta + 1;
				}

			}
			ai = ai / Math.Max (1, cpta);

			for (double k = 0.0; k < cptlabel; k++)
			{
				
				if (labels [i] != k)
				{
					double dist = 0;
					int cptb = 0;
					for (int l = 0; l < datacount; l++) 
					{
						
						if (k == labels [l])
						{
							dist +=  Distance.Distance (data[i], data[l]); 
							cptb = cptb + 1;
						}

						
					}

					dist = dist / cptb;
					if (dist < bi)
					{
						bi = dist;
					}
				
				}
			}


			double si = (bi - ai) / Math.Max (ai, bi);

			SilhouetteScore += si;

	}

		SilhouetteScore = SilhouetteScore / datacount;
		return SilhouetteScore;
}
}

