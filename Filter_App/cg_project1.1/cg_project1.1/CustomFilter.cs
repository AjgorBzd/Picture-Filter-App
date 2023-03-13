using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cg_project1._1
{
    class CustomFilter : ConvolutionFilterBase
    {
        private string filtername;
        public override string FilterName
        {
            get { return filtername; }
        }

        private int offr;
        public override int OffsetRed
        {
            get { return offr; }
        }

        private int offg;
        public override int OffsetGreen
        {
            get { return offg; }
        }

        private int offb;
        public override int OffsetBlue
        {
            get { return offb; }
        }

        private int anchor_row;
        public override int AnchorRow
        {
            get { return anchor_row; }
        }

        private int anchor_col;
        public override int AnchorCol
        {
            get { return anchor_col; }
        }

        private double divisor;
        public override double Divisor
        {
            get { return divisor; }
        }

        private double[,] filterMatrix;

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }

        public CustomFilter(string name, int or, int og, int ob, int ar, int ac, double div, double[,] matrix)
        {
            filtername = name;
            offr = or;
            offg = og;
            offb = ob;
            anchor_row = ar;
            anchor_col = ac;
            divisor = div;
            filterMatrix = matrix;
        }
    }
}
