namespace Matrix_Filters {
    public class Filter {
        public Matrix3X3 FilterMatrix { get; set; }

        public int Shift { get; set; }

        public int Divisor { get; set; }

        public string Name { get; set; }

        public Filter() {}

        public Filter(Filter toCopy)
        {
            Name = toCopy.Name;
            Shift = toCopy.Shift;
            Divisor = toCopy.Divisor;
            FilterMatrix = toCopy.FilterMatrix.Clone();
        }

        public Filter Clone()
        {
            return new Filter(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
