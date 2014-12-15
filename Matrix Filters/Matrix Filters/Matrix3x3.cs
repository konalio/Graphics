using System;
using System.Collections.Generic;

namespace Matrix_Filters {
    public class Matrix3X3 {
        private readonly List<List<int>> _matrix;

        //Properties for easier XAML binding in WPF
        public int TopLeft {
            get { return _matrix[0][0]; }
            set { _matrix[0][0] = value; }
        }

        public int TopMid {
            get { return _matrix[0][1]; }
            set { _matrix[0][1] = value; }
        }
        public int TopRight {
            get { return _matrix[0][2]; }
            set { _matrix[0][2] = value; }
        }
        public int MidLeft {
            get { return _matrix[1][0]; }
            set { _matrix[1][0] = value; }
        }
        public int MidMid {
            get { return _matrix[1][1]; }
            set { _matrix[1][1] = value; }
        }
        public int MidRight {
            get { return _matrix[1][2]; }
            set { _matrix[1][2] = value; }
        }
        public int BotLeft {
            get { return _matrix[2][0]; }
            set { _matrix[2][0] = value; }
        }
        public int BotMid {
            get { return _matrix[2][1]; }
            set { _matrix[2][1] = value; }
        }
        public int BotRight {
            get { return _matrix[2][2]; }
            set { _matrix[2][2] = value; }
        }

        public Matrix3X3(int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9) {
            _matrix = new List<List<int>>
            {
                new List<int> {v1, v2, v3},
                new List<int> {v4, v5, v6},
                new List<int> {v7, v8, v9}
            };
        }

        public Matrix3X3(List<int> valueList) {
            if (valueList == null) throw new ArgumentNullException();
            if (valueList.Count != 9) throw new Exception("Incorrect number of elements.");

            _matrix = new List<List<int>>();

            for (var i = 0; i < 3; i++) {
                _matrix.Add(new List<int>());
                for (var j = 0; j < 3; j++) {
                    _matrix[i].Add(valueList[i * 3 + j]);
                }
            }
        }

        public Matrix3X3 Clone() {
            var resultList = new List<int>();
            for (var i = 0; i < 3; i++) {
                for (var j = 0; j < 3; j++) {
                    resultList.Add(_matrix[i][j]);
                }
            }
            return new Matrix3X3(resultList);
        }

        public List<int> this[int key] {
            get { return _matrix[key]; }
        }
    }
}
