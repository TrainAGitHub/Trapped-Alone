using System;

public class Class1
{
    public struct Bullet
    {
        public double X;
        public double Y;
        public string Type;
        public string Alive;
        public int Size;
        public int DestinyX;
        public int DestinyY;
        public double Speed;
        public Ellipse ellipsebullet;
        SolidColorBrush colorOnBullet;
        public Bullet(int x, int y, string type, string alive, int destinyx, int destinyy, double speed, Ellipse ecBullet, SolidColorBrush bulletcolor)
        {
            X = x;
            Y = y;
            Type = type;
            Alive = alive;
            Size = 10;
            DestinyX = destinyx;
            DestinyY = destinyy;
            Speed = speed;
            ellipsebullet = ecBullet;// = new Ellipse();
            colorOnBullet = bulletcolor;
        }
    }
}
