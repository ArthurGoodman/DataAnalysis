﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAnalysis {
    class Perceptron : IAlgorithm {
        private static int maxLearningEpochs = 10000;

        private string info = "No info.";
        public string Info {
            get {
                return info;
            }
        }

        private double c = 1;
        public double Param {
            get {
                return c / 2;
            }

            set {
                c = Math.Max(0, Math.Min(value, 1)) * 2;
            }
        }

        public DataSet Samples { get; set; }

        private Vector[] w;
        private Sample[] data;

        private int iterations;

        public void Run() {
            Initialize();

            for (iterations = 0; iterations < maxLearningEpochs && !Learn(); iterations++) {
            }

            Mark();
            BuildInfo();
        }

        private int MaxIndex(IEnumerable<double> set) {
            double max = double.MinValue;
            int maxIndex = -1;

            int i = 0;
            foreach (double d in set) {
                if (d > max) {
                    max = d;
                    maxIndex = i;
                }

                i++;
            }

            return maxIndex;
        }

        private delegate void ActionFunction(int index, int maxIndex);

        private void Iterate(ActionFunction f) {
            for (int i = 0; i < data.Length; i++)
                f(i, MaxIndex(w.Select(v => v.Dot(data[i].Vector))));
        }

        private void Initialize() {
            w = new Vector[Samples.CountClusters()];

            for (int i = 0; i < w.Length; i++)
                w[i] = new Vector(5);

            data = Samples.Data.Select(s => s.CloneExpanded()).ToArray();
        }

        private bool Learn() {
            bool corrected = false;

            Iterate((index, maxIndex) => {
                if (maxIndex != data[index].Cluster) {
                    corrected = true;

                    Vector delta = data[index].Vector * c;

                    w[data[index].Cluster] += delta;
                    w[maxIndex] -= delta;
                }
            });

            return !corrected;
        }

        private void Mark() {
            Iterate((index, maxIndex) => Samples[index].Mark = maxIndex != data[index].Cluster);
        }

        private void BuildInfo() {
            info = string.Format("{0} iterations\n\n", iterations);

            int i = 0;
            foreach (Vector v in w)
                info += string.Format("d{0}(x) = {1:g4}*x1 {2} {3:g4}*x2 {4} {5:g4}*x3 {6} {7:g4}*x4 {8} {9:g4}\n", i++, v[0], v[1] > 0 ? '+' : '-', Math.Abs(v[1]), v[2] > 0 ? '+' : '-', Math.Abs(v[2]), v[3] > 0 ? '+' : '-', Math.Abs(v[3]), v[4] > 0 ? '+' : '-', Math.Abs(v[4]));
        }
    }
}
