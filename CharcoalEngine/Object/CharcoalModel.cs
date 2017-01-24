using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharcoalEngine.Object
{
    public class CharcoalModel
    {
        public string FileName;
        public List<Mesh> Meshes = new List<Mesh>();
    }
    public class Mesh
    {
        public List<Part> Parts = new List<Part>();
    }
    public class Part
    {

    }
}
