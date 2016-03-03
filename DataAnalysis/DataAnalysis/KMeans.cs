﻿using System;

namespace DataAnalysis {
    public class KMeans : IAlgorithm {
        public int ClusterCount {
            get {
                return k;
            }
        }

        private int k = 3;
        public double Param {
            get {
                return k / 20.0;
            }

            set {
                k = Math.Max(1, (int)(value * 20));
            }
        }

        public Sample[] Samples { get; set; }

        public string Info {
            get {
                throw new NotImplementedException();
            }
        }

        private CenterSet centers;
        private CenterSet newCenters;

        public KMeans() {
            centers = new CenterSet(this);
            newCenters = new CenterSet(this);
        }

        private void Swap() {
            CenterSet temp = centers;
            centers = newCenters;
            newCenters = temp;
        }

        public void Run() {
            if (Samples.Length == 0)
                return;

            centers.Reset();

            foreach (Sample s in Samples)
                s.Reset();

            for (int i = 0; i < k; i++)
                centers.Add(Samples[i].Clone());

            while (true) {
                foreach (Sample s in Samples)
                    centers.Assign(s);

                newCenters.Reset();

                int[] clusterSizes = new int[k];

                for (int i = 0; i < k; i++) {
                    newCenters.Add(centers[i].Clone());
                    clusterSizes[i] = 1;
                }

                foreach (Sample s in Samples) {
                    newCenters[s.Cluster].Vector += s.Vector;
                    clusterSizes[s.Cluster]++;
                }

                for (int i = 0; i < k; i++)
                    newCenters[i].Vector /= clusterSizes[i];

                int j = 0;

                for (; j < k; j++)
                    if (!newCenters[j].Vector.Equals(centers[j].Vector))
                        break;

                if (j == k)
                    break;

                Swap();
            }
        }
    }
}
