using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using CharcoalEngine.Scene;
using CharcoalEngine;
using CharcoalEngine.Utilities;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows;
using System.Design;

namespace CharcoalEngine.Object
{
    public class AnimationRig : Transform
    {
        List<Transform> Joints
        {
            get
            {
                if (TargetObject == -1) return null;
                if ((Children.Count - 1) < TargetObject) return null;
                List<Transform> T = new List<Transform>();
                T = Children.ToList();
                T.RemoveAt(TargetObject);
                return T;
            }
        }
        Transform Target
        {
            get
            {
                if (TargetObject == -1) return null;
                if ((Children.Count - 1) >= TargetObject) return Children[TargetObject];
                return null;
            }
            set
            {
                if (TargetObject != -1) { if ((Children.Count - 1) >= TargetObject) Children[TargetObject] = value; }
            }
        }


        public float AnimationSpeed
        {
            get { return __animationspeed__; }
            set { __animationspeed__ = value; }
        }
        float __animationspeed__ = 0.01f;

        public bool DrawPath
        {
            get; set;
        } = true;

        public bool Update_Rig { get; set; } = true;

        public int Joint = 0;//current joint
        float amount = 0;

        public int TargetObject { get; set; } = 0;

        public AnimationRig()
        {
            Name = "AnimationRig";
        }

        public override void Update()
        {
            if (Update_Rig)
            {
                List<Transform> js = Joints;
                if (TargetObject != -1)
                {
                    if (js != null)
                    {
                        if (js.Count > 1)
                        {
                            Target.Position = Vector3.CatmullRom(js[PreviousJoint(Joint, js)].Position, js[Joint].Position, js[NextJoint(Joint, js)].Position, js[NextJoint(NextJoint(Joint, js), js)].Position, amount);
                            Target.YawPitchRoll = Vector3.CatmullRom(js[PreviousJoint(Joint, js)].YawPitchRoll, js[Joint].YawPitchRoll, js[NextJoint(Joint, js)].YawPitchRoll, js[NextJoint(NextJoint(Joint, js), js)].YawPitchRoll, amount);

                            amount += AnimationSpeed;
                            if (amount > 1)
                            {
                                amount = 0;
                                Joint++;
                                if (Joint > js.Count - 1)
                                    Joint = 0;
                            }

                        }
                    }
                }
            }
            base.Update();
        }

        int PreviousJoint(int joint, List<Transform> j)
        {
            if (joint -  1 > -1)
                return 0;
            else
                return j.Count-1;
        }

        int NextJoint(int joint, List<Transform> j)
        {
            if (joint + 1 == j.Count)
                return 0;
            else
                return joint + 1;
        }

        public override void Draw(Effect e)
        {
            List<Transform> js = Joints;//the resons for always using this local 
                                        //variable is so that the Joints list does not have to be regenerated upon every single reference

            if (Joints.Count > 1 && DrawPath == true)
            {
                int j = 0;
                float am = 0;
                Vector3 Start, End;
                Start = Joints[0].AbsolutePosition;

                foreach (Transform J in Joints)
                {
                    for (am = 0; am < 1.0f; am += 0.01f)
                    {

                        End = Vector3.CatmullRom(Joints[PreviousJoint(j, js)].AbsolutePosition, Joints[j].AbsolutePosition, Joints[NextJoint(j, js)].AbsolutePosition, Joints[NextJoint(NextJoint(j, js), js)].AbsolutePosition, am);

                        LineUtility3D.Draw3DLine(Engine.g, Camera.View, Camera.Projection, Microsoft.Xna.Framework.Color.Gray, Start, End);
                        Start = End;
                    }
                    j++;
                }
            }
            Target.Draw(e);
            base.Draw(e);
        }

    }
}
