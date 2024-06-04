using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Component
{
    internal class getIdController
    {
        public static class TemporaryDataManager
        {
            private static string selectedId;

            public static string SelectedId
            {
                get { return selectedId; }
                set { selectedId = value; }
            }
        }

    }
}
