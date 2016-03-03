﻿namespace DataAnalysis {
    class Matrix4x4 {
        private double[,] data;

        public Matrix4x4(double[,] data) {
            this.data = data;
        }

        public void Map(Vector4 v) {
            double[] mapped = new double[4];

            for (int i = 0; i < 4; i++) {
                mapped[i] = 0;

                for (int j = 0; j < 4; j++)
                    mapped[i] += data[i, j] * v[j];
            }

            for (int i = 0; i < 4; i++)
                v[i] = mapped[i];
        }
    }
}