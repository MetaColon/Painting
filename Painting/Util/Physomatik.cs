using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Painting.Util
{
    public class Physomatik
    {
        public static double DensityAir = 1.2041;
        public static double G = 9.81;
        public static double Pi = 3.14159265358979323;

        public static double GetDeltaSpeed(double a, double dt) => a*dt; //this one is obvious

        public static List<int> GetIndexesOfX(List<char> listi, char x)
        {
            var arri = listi.ToArray();
            var ints = new List<int>();
            for (var i = 0; i < arri.Length; i++)
                if (arri[i] == x)
                    ints.Add(i);
            return ints;
        }

        public static double[] GetNewPos(double[] v, double[] pos, double step)
            => new double[2] {pos[0] + GetXPart(v)*step, pos[1] + GetYPart(v)*step};

        public static double[] GetNewSpeedAfterImpact(double[] v0, double step, double c_w, double A, double P, double m,
            double g, double t, double f, double angle)
        {
            double vx = GetXPart(v0), vy = GetYPart(v0);
            var F = GetF(getParts(v0[0], (v0[1] + 180)%360, (angle + 90)%360, angle)[0, 0], t, m);
            var F_N = getF_N(angle, m, g) + getParts(F, (v0[1] + 180)%360, (angle + 90)%360, angle)[0, 0];
            var F_H = getF_H(angle, m, g);
            var F_L = getF_L(c_w, A, P, v0[0]);
            var F_R = getF_R(f, F_N);
            var F_Resa =
                GetResVector(new double[3, 2]
                    {{F_H, (angle + 180)%360}, {F_L, (v0[1] + 180)%360}, {F_R, (v0[1] + 180)%360}});
            var F_Res = new double[2]
            {
                getParts(F_Resa[0], F_Resa[1], angle, angle + 90)[0, 0],
                getParts(F_Resa[0], F_Resa[1], angle, angle + 90)[0, 1]
            };
            vx += GetXPart(F_Res)/m*step;
            vy += GetYPart(F_Res)/m*step;
            return GetResVector(new double[2, 2] {{vx, 0}, {vy, 90}});
        }

        public static double[] GetNewSpeedAfterImpactM(double[] v0, double step, double c_w, double A, double P,
            double m, double g, double t, double f)
        {
            double vx = GetXPart(v0), vy = GetYPart(v0);
            var fN = getF_G(m, g) + GetF(vy, t, m);
            if (vx > 0)
                vx += GetA(-f*fN - getF_L(c_w, A, P, vx), m)*step;
            else
                vx += GetA(+f*fN + getF_L(c_w, A, P, vx), m)*step;
            return GetResVector(new double[2, 2] {{vx, 0}, {0, 90}});
        }

        public static List<double[]> GetSimulatedPossesFromFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var listi = content.ToList();
            listi.Remove('[');
            listi.Remove(']');
            string.Join(string.Empty, listi);
            var tupleStrings = ToListByBrackets(listi);
            return tupleStrings.Select(StringToDoubles).ToList();
        }

        public static double[] StringToDoubles(string content)
        {
            var splitted = content.Split(',');
            return new double[2] {Convert.ToDouble(splitted[0]), Convert.ToDouble(splitted[1])};
        }

        public static List<string> ToListByBrackets(List<char> listi)
        {
            var open = GetIndexesOfX(listi, '(');
            var close = GetIndexesOfX(listi, ')');
            var final = new List<string>();
            while ((listi.Count > 0) && (close.Count > 0) && (open.Count > 0))
            {
                final.Add(string.Join(string.Empty, listi.GetRange(open.First() + 1, close.First() - open.First() - 1)));
                open.Remove(open.First());
                close.Remove(close.First());
            }
            return final;
        }

        #region resVector

        public static double[] GetResVector(double[,] vectors)
        {
            double xsum = 0, ysum = 0;
            var vector = new double[2];
            for (var i = 0; i < vectors.GetLength(0); i++)
            {
                vector[0] = vectors[i, 0];
                vector[1] = vectors[i, 1];
                xsum += GetXPart(vector); //get the parts...
                ysum += GetYPart(vector);
            }
            vector[0] = Math.Sqrt(xsum*xsum + ysum*ysum); //and add the parts
            vector[1] = GetResAngle(xsum, ysum); //and get the angle
            if (vector[1] < 0)
                vector[1] = 360 + vector[1];
            vector[1] %= 360;
            return vector;
        }

        public static double GetResAngle(double x, double y)
        {
            if (x > 0) //this is ugly code. But I think, there's no better way to do this
                return ToDegree(Math.Atan(y/x));
            if (Math.Abs(x) < 0.001)
                return Math.Sign(y)*90;
            if (Math.Abs(y) < 0.001)
                return 180;
            if (y > 0)
                return 180 - ToDegree(Math.Atan(Math.Abs(y)/Math.Abs(x)));
            return 180 + ToDegree(Math.Atan(Math.Abs(y)/Math.Abs(x)));
        }

        #endregion

        #region Shot

        public static double[] GetSpeedAtShot(double F_Wurf, double angle_Wurf, double m, double g, double c_w, double A,
                double P, double t, double t_throw, double step)
            //you should use this part if you forgot your current position/speed
        {
            double F_G = getF_G(m, g), F_L = 0;
            double[] v = new double[2] {0, 0}, F_res = new double[2];
            for (double i = 0; i < t; i += step)
            {
                F_L = getF_L(c_w, A, P, v[0]);
                var Fs = i <= t_throw
                    ? new double[3, 2] {{F_G, 270}, {F_Wurf, angle_Wurf}, {F_L, (v[1] + 180)%360}}
                    : new double[2, 2] {{F_G, 270}, {F_L, (v[1] + 180)%360}};
                F_res = GetResVector(Fs);
                v = GetNewSpeed(v, F_res, step, m);
            }
            return v;
        }

        public static double[] GetPosAtShot(double F_Wurf, double angle_Wurf, double m, double g, double c_w, double A,
            double P, double t, double t_throw, double step)
        {
            double[] pos = new double[2], speed = new double[2];
            for (double i = 0; i < t; i += step)
            {
                speed = GetSpeedAtShot(F_Wurf, angle_Wurf, m, g, c_w, A, P, i, t_throw, step);
                pos[0] += GetXPart(speed)*step;
                pos[1] += GetYPart(speed)*step;
            }
            return pos;
        } //as well as this

        public static double[,] GetNewPos_Speed(double[] F_Wurf, double m, double g, double c_w, double A, double P,
                double step, double[] oldpos, double[] oldv)
            //this gives you a new position/speed - it's stepwise because air is bad
        {
            var vectors = new double[3, 2]
                {{F_Wurf[0], F_Wurf[1]}, {getF_G(m, g), 270}, {getF_L(c_w, A, P, oldv[0]), (oldv[1] + 180)%360}};
            var newSpeed = GetNewSpeed(oldv, GetResVector(vectors), step, m);
            var pos = new double[2] {GetXPart(newSpeed)*step + oldpos[0], GetYPart(newSpeed)*step + oldpos[1]};
            return new double[2, 2] {{pos[0], pos[1]}, {newSpeed[0], newSpeed[1]}};
        }

        public static double[] GetPosAtShotWithS(double F_Wurf, double angle_Wurf, double m, double g, double c_w,
            double A, double P, double t, double s_throw, double step)
        {
            double[] pos = new double[2], speed = new double[2];
            var t_throw = t;
            for (double i = 0; i < t; i += step)
            {
                if ((Math.Sqrt(pos[0]*pos[0] + pos[1]*pos[1]) > s_throw) && (t_throw == t))
                    t_throw = i;
                speed = GetSpeedAtShot(F_Wurf, angle_Wurf, m, g, c_w, A, P, i, t_throw, step);
                pos[0] += GetXPart(speed)*step;
                pos[1] += GetYPart(speed)*step;
            }
            return pos;
        } //like this one

        public static double GetSpeedWithAir(double A, double F, double P, double c_w, double t, double m)
            //this formula only works with positive or not too negative F
            => (Math.Sqrt(2*c_w*P*t*t*A*F + m*m) - m)/c_w*P*t*A;

        public static double GetSpeedWithAirAndStart(double A, double F, double P, double c_w, double t, double v0,
                double m) //...just like this
            => (Math.Sqrt(2*A*F*P*c_w*t*t + 2*A*P*c_w*v0*m*t + m*m) - m)/c_w*P*t*A;

        #endregion

        #region getParts

        public static double GetOneXPart(double res, double resangle, double otherangle, double ownangle)
        {
            double A = ToRadian(otherangle), B = ToRadian(resangle), C = ToRadian(ownangle);
            return (res*Math.Tan(A)*Math.Cos(B) - res*Math.Sin(B))/(Math.Tan(A) - Math.Tan(C));
        }

        public static double[,] GetXParts(double res, double resangle, double angle1, double angle2)
        {
            return new double[2, 2]
            {
                {GetOneXPart(res, resangle, angle2, angle1), angle1},
                {GetOneXPart(res, resangle, angle1, angle2), angle2}
            };
        }

        public static double[,] getParts(double res, double resangle, double angle1, double angle2)
        {
            var xParts = GetXParts(res, resangle, angle1, angle2);
            return new double[2, 2]
            {
                {xParts[0, 0]/Math.Cos(ToRadian(xParts[0, 1])), xParts[0, 1]},
                {xParts[1, 0]/Math.Cos(ToRadian(xParts[1, 1])), xParts[1, 1]}
            };
        }

        #endregion

        #region getF

        public static double getF_G(double m, double g) //return the fitting F_s
            => m*g;

        public static double getF_L(double cW, double A, double P, double v) => 0.5*cW*A*P*v*v;

        public static double getF_N(double angle, double m, double g) => Math.Cos(ToRadian(angle))*getF_G(m, g);

        public static double getF_H(double angle, double m, double g) => Math.Sin(ToRadian(angle))*getF_G(m, g);

        public static double getF_R(double f, double angle, double m, double g) => getF_R(f, getF_N(angle, m, g));

        public static double getF_R(double f, double F_N) => f*F_N;

        #endregion

        #region Hill

        public static double GetNewSpeedAtHill(double f, double angle, double m, double g, double F_S, double step,
                double[] v0, double c_w, double A, double P)
            //it's nearly the same as the shot - you just can't fall down and have more resistance
        {
            var v = v0[0];
            if ((v0[1] > (angle + 180)%360 - 5) && (v0[1] < (angle + 180)%360 + 5))
                v *= -1;
            if (v > 0)
                return v + (F_S - getF_H(angle, m, g) - getF_R(f, angle, m, g) - getF_L(c_w, A, P, v))/m*step;
            return v + (F_S - getF_H(angle, m, g) + getF_R(f, angle, m, g) + getF_L(c_w, A, P, v))/m*step;
        }

        public static double[] GetNewPosAtHill(double f, double angle, double m, double g, double F_S, double step,
            double[] v0, double c_w, double A, double P, double[] pos0)
        {
            var newpos = new double[2] {pos0[0], pos0[1]};
            var newspeed = GetNewSpeedAtHill(f, angle, m, g, F_S, step, v0, c_w, A, P);
            newpos[0] += GetXPart(new[] {newspeed*step, angle});
            newpos[1] += GetYPart(new[] {newspeed*step, angle});
            return newpos;
        }

        public static double[,] GetNewPos_SpeedAtHill(double f, double angle, double m, double g, double F_S,
            double step, double[] v0, double c_w, double A, double P, double[] pos0)
        {
            var newSpeed = new double[2] {GetNewSpeedAtHill(f, angle, m, g, F_S, step, v0, c_w, A, P), angle};
            if (!(newSpeed[0] < 0))
                return new double[2, 2]
                {
                    {
                        GetNewPosAtHill(f, angle, m, g, F_S, step, v0, c_w, A, P, pos0)[0],
                        GetNewPosAtHill(f, angle, m, g, F_S, step, v0, c_w, A, P, pos0)[1]
                    },
                    {newSpeed[0], newSpeed[1]}
                };
            newSpeed[0] *= -1;
            newSpeed[1] = (newSpeed[1] + 180)%360;
            return new double[2, 2]
            {
                {
                    GetNewPosAtHill(f, angle, m, g, F_S, step, v0, c_w, A, P, pos0)[0],
                    GetNewPosAtHill(f, angle, m, g, F_S, step, v0, c_w, A, P, pos0)[1]
                },
                {newSpeed[0], newSpeed[1]}
            };
        }

        #endregion

        #region newSpeed

        public static double[] GetNewSpeed(double[] vectorv, double[] F, double dt, double m)
            //returns the new Speed in relativity to the old one
            => GetResVector(new double[2, 2]
            {
                {GetNewSpeed(GetXPart(vectorv), GetXPart(F), dt, m), 0},
                {GetNewSpeed(GetYPart(vectorv), GetYPart(F), dt, m), 90}
            });

        public static double GetNewSpeed(double v0, double F, double dt, double m) => v0 + GetDeltaSpeed(GetA(F, m), dt);

        public static double GetNewSpeed(double v0, double a, double dt) => v0 + GetDeltaSpeed(a, dt);

        #endregion

        #region FAM

        public static double GetF(double v, double t, double m) => v/t*m;

        public static double GetA(double F, double m) => F/m;

        #endregion

        #region getPart

        public static double GetYPart(double[] vector) //returns the y-/x-Part
            => Math.Sin(ToRadian(vector[1]))*vector[0];

        public static double GetXPart(double[] vector) => Math.Cos(ToRadian(vector[1]))*vector[0];

        #endregion

        #region ToRadian/Degree

        public static double ToRadian(double degree) //why does C# use radians?
            => degree*Pi/180;

        public static double ToDegree(double radian) => radian*180/Pi;

        #endregion
    }
}