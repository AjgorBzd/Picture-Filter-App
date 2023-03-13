using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cg_project1._1
{
    class Emboss : ConvolutionFilterBase
    {
        public override string FilterName
        {
            get { return "Emboss"; }
        }

        private int offr = 0;
        public override int OffsetRed
        {
            get { return offr; }
        }

        private int offg = 0;
        public override int OffsetGreen
        {
            get { return offg; }
        }

        private int offb = 0;
        public override int OffsetBlue
        {
            get { return offb; }
        }

        private int anchor_row = 2;
        public override int AnchorRow
        {
            get { return anchor_row; }
        }

        private int anchor_col = 2;
        public override int AnchorCol
        {
            get { return anchor_col; }
        }

        private double divisor = 1;
        public override double Divisor
        {
            get { return divisor; }
        }

        private double[,] filterMatrix =
            new double[,] { { -1.0, -1.0, 0.0, },
                            { -1.0, 1.0, 1.0, },
                            { 0.0, 1.0, 1.0, }, };

        public override double[,] FilterMatrix
        {
            get { return filterMatrix; }
        }
    }
}
