using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;


namespace Reconocimiento_facial
{
    // Definir la clase DBCon
    public class DBCon
    {
        // Crear un campo para almacenar la conexión a la base de datos
        private OleDbConnection conn;

        // Crear una lista para almacenar datos de usuarios
        public List<UserData> Users = new List<UserData>();

        // Constructor de la clase, recibe la cadena de conexión a la base de datos
        public DBCon(string connectionString)
        {
            // Inicializar la conexión utilizando la cadena de conexión proporcionada
            conn = new OleDbConnection(connectionString);
            // Abrir la conexión
            conn.Open();
        }
    }
    public bool GuardarImagen(string Name, string Code, byte[] abImagen)
    {
        try
        {
            using (OleDbCommand comm = new OleDbCommand("INSERT INTO UserFaces (Name, Code, Face) VALUES (?, ?, ?)", conn))
            {
                comm.Parameters.AddWithValue("@Name", Name);
                comm.Parameters.AddWithValue("@Code", Code);
                comm.Parameters.AddWithValue("@Face", abImagen);
                int iResultado = comm.ExecuteNonQuery();
                return iResultado > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }
    public DataTable ObtenerBytesImagen()
        {
            string sql = "SELECT IdImage,Name,Code,Face FROM UserFaces";
            OleDbDataAdapter adaptador = new OleDbDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            int cont = dt.Rows.Count;
            Name = new string[cont];

            for (int i = 0; i < cont; i++)
            {
                Name[i] = dt.Rows[i]["Name"].ToString();
                face = (byte[])dt.Rows[i]["Face"];
                Face.Add(face);
            }
            TotalUser = dt.Rows.Count;
            conn.Close();
            return dt;
        }

        public void ConvertImgToBinary(string Name, string Code, Image Img)
        {
            Bitmap bmp = new Bitmap(Img);
            MemoryStream MyStream = new MemoryStream();
            bmp.Save(MyStream, System.Drawing.Imaging.ImageFormat.Bmp);

            byte[] abImagen = MyStream.ToArray();
            GuardarImagen(Name, Code, abImagen);
        }

        public Image ConvertByteToImg( int con)
        {
            Image FetImg;
            byte[] img = Face[con];
            MemoryStream ms = new MemoryStream(img);
            FetImg = Image.FromStream(ms);
            ms.Close();
            return FetImg;

        }
    }
}
