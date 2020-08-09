using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;


namespace SpaceSceneOpenTK
{
    struct Vertex : IEquatable<Vertex>
    {
        public int ver;
        public int tex;

        public bool Equals(Vertex other)
        {
            if (this.ver == other.ver && this.tex == other.tex)
                return true;
            return false;
        }
    }

    public class ImporterOBJ 
    {
        public uint[] index { get; private set; }
        public float[] vertices { get; private set; }
        public float[] textCoord { get; private set; }
  
        public ImporterOBJ(string filepath)
        {
            List<float> ver = new List<float>();
            List<float> tex = new List<float>();
            List<string> ind = new List<string>();
            List<Vertex> rawInd = new List<Vertex>();
            var englishCulture = CultureInfo.GetCultureInfo("en-US");

            List<float> verBuf = new List<float>();
            List<float> texBuf = new List<float>();

            try
            {
                string temp;
                Regex NumReg = new Regex(@"-*[0-9]+(\.[0-9]*)?"); //regex for numbers
                MatchCollection Matches; //matches collection for regex
                using (StreamReader file = new StreamReader(filepath))
                {
                    while(!file.EndOfStream)
                    {
                        temp = file.ReadLine();
                        if (temp.StartsWith("v ")) //string with vertices
                        {
                            Matches = NumReg.Matches(temp);
                            foreach (Match m in Matches)
                            {
                                ver.Add(float.Parse(m.Value, englishCulture));
                            }
                        }
                        else if (temp.StartsWith("vt ")) //string with texture coordinates
                        {
                            Matches = NumReg.Matches(temp);
                            foreach (Match m in Matches)
                            {
                                tex.Add(float.Parse(m.Value, englishCulture));
                            }
                        }
                        else if (temp.StartsWith("f ")) //string with indicies
                        {
                            Matches = NumReg.Matches(temp);
                            Vertex vert = new Vertex();
                            for (int i = 0; i < Matches.Count; i += 2)
                            {
                                vert.ver = int.Parse(Matches[i].Value, englishCulture) - 1;
                                vert.tex = int.Parse(Matches[i + 1].Value, englishCulture) - 1;
                                rawInd.Add(vert);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("все пошло по пизде");
            }

            foreach (Vertex item in rawInd)
            {
                verBuf.Add(ver[item.ver * 3]);
                verBuf.Add(ver[item.ver * 3 + 1]);
                verBuf.Add(ver[item.ver * 3 + 2]);

                verBuf.Add(tex[item.tex * 3]);
                verBuf.Add(tex[item.tex * 3 + 1]);
                verBuf.Add(tex[item.tex * 3 + 2]);

                texBuf.Add(tex[item.tex * 3]);
                texBuf.Add(tex[item.tex * 3 + 1]);
                texBuf.Add(tex[item.tex * 3 + 2]);  
            }

            index = new uint[rawInd.Count];
            for (uint i = 0; i < index.Length; i++){ index[i] = i; };
            vertices = verBuf.ToArray();
            textCoord = texBuf.ToArray();
        }

    }
}
