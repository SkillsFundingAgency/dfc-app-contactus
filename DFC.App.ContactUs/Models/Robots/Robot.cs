<<<<<<< HEAD
﻿using System.Text;

namespace DFC.App.ContactUs.Models.Robots
{
=======
﻿using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DFC.App.ContactUs.Models.Robots
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class Robot
    {
        private readonly StringBuilder robotData;

        public Robot()
        {
            robotData = new StringBuilder();
        }

        public string Data => robotData.ToString();

        public void Add(string text)
        {
            robotData.AppendLine(text);
        }
    }
}
