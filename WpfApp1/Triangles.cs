namespace WpfApp1
{
    internal class Triangle
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }
        public bool IsValid { get; set; }
        public string? Message { get; set; }

        public Triangle(double a, double b, double c)
        {
            SideA = a;
            SideB = b;
            SideC = c;
            IsValid = IsTriangleValid(a, b, c);
        }

        private bool IsTriangleValid(double a, double b, double c)
        {
            return (a + b > c) && (b + c > a) && (c + a > b);
        }
    }
}
