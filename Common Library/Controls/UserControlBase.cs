using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace DamirM.CommonLibrary
{
    public class UserControlBase : UserControl, IEnumerable<UserControlBase>
    {

        #region ControlMembers
        private UserControlBase downControl;
        private UserControlBase upControl;
        private ArrayList childList;
        private UserControlBase parent;

        private int fetchNextChildCounter = 0; // curent child position index
        private int distanceBetweenControl = 5;
        private int childLeftOfSet = 20;
        private int defaultTop = 0;
        private int defaultLeft = 0;

        public delegate void delUserControlBaseGenericDelegate(Control control);
        public event delUserControlBaseGenericDelegate OnRemove; 
        #endregion


        #region ControlsMethods
        public UserControlBase(int distanceBetweenControl)
        {
            this.distanceBetweenControl = distanceBetweenControl;
            this.defaultTop = this.Top;
            this.defaultLeft = this.Left;
        }
        public UserControlBase()
        {
            this.LocationChanged += new EventHandler(UserControlBase_LocationChanged);
        }

        private void UserControlBase_LocationChanged(object sender, EventArgs e)
        {
            // Set default top and left locations when location change but only if it is first control in line. 
            // These values will always setirati when creating the control but UpControl property logic that has taken over control of these values
            if (this.upControl == null)
            {
                this.defaultTop = this.Top;
                this.defaultLeft = this.Left;
            }
        }

        public void AddChild(UserControlBase childControl)
        {
            // if child control is null then exit
            if (childControl == null)
            {
                return;
            }


            // check if child list array not null
            if (childList == null)
            {
                // child list is null so we will make new instance
                // and set upControl to this parent
                childList = new ArrayList();
                childControl.upControl = this;
            }
            else
            {

                // check if control is not already in child list
                if (IsChild(childControl))
                {
                    // control is already child from this instance, exit
                    return;
                }

                // this is not firs child from this parent
                // so we will set topControl from uper child of this parent
                //UserControlBase upperChild = (UserControlBase)childList[childList.Count - 1];
                UserControlBase lastChild = GetLastChild();
                lastChild.downControl = childControl;
                childControl.upControl = lastChild;
            }

            //// if new child control is equals as down control object
            //if (this.downControl.Equals(childControl))
            //{
            //    //  make new reference to down control of child control
            //    this.downControl = childControl.downControl;
            //    // remove referenc from child down control
            //    childControl.downControl = null;
            //    // TODO: add down control top control to this
            //}

            // childs down control reference will be removed
            if (childControl.downControl != null)
            {
                childControl.downControl.upControl = this;
                this.downControl = childControl.downControl;
                childControl.downControl = null;
            }
            else
            {
                // if child dont have down control then kill reference from this parent
                childControl.downControl = null;
                this.downControl = null;
            }


            // set this as parent and add it to child list
            childControl.ParentControl = this;
            //childList.Add(childControl);

            childControl.AlignSelf();
            //this.OnMove(null);
        }

        /// <summary>
        /// This method will return instance of last child of last child from this instace, if child count is zero then will return self instance
        /// </summary>
        /// <returns></returns>
        private UserControlBase GetLastControlInLine()
        {
            // chech if child list object is not null
            if (childList != null)
            {
                // check if this control have childs
                if (childList.Count > 0)
                {
                    // get last child 
                    UserControlBase child = (UserControlBase)childList[childList.Count - 1];

                    // check if this child have children
                    if (child.HasChildrenControls)
                    {
                        // now get last in line  from all children
                        return child.GetLastControlInLine();
                    }
                    else
                    {
                        // child dont have childrens so return just this child
                        return (UserControlBase)childList[childList.Count - 1];
                    }
                }
                else
                {
                    // control dont have childs, return self instance
                    return this;
                }
            }
            else
            {
                // child list object is null so this control have no child, return self instance
                return this;
            }
        }

        /// <summary>
        /// Return last child from this instance, if child count is zero then will return self instance
        /// </summary>
        /// <returns></returns>
        private UserControlBase GetLastChild()
        {
            // chech if child list object is not null
            if (childList != null)
            {
                // check if this control have childs
                if (childList.Count > 0)
                {
                    // get last child 
                    return (UserControlBase)childList[childList.Count - 1];
                }
                else
                {
                    // control dont have childs, return self instance
                    return this;
                }
            }
            else
            {
                // child list object is null so this control have no child, return self instance
                return this;
            }
        }

        /// <summary>
        /// Check if control is child control from this instance
        /// </summary>
        /// <param name="control">Control to check</param>
        /// <returns>return true if child control is child</returns>
        private bool IsChild(UserControlBase control)
        {
            bool result = false;
            foreach (UserControlBase child in childList)
            {
                if (child.Equals(control))
                {
                    result = true;
                    break;
                }

            }
            return result;
        }
        /// <summary>
        /// Executes align themselves and all their children and down controls
        /// </summary>
        public void AlignSelf()
        {
            UserControlBase lastControlInLine = null;
            if (this.upControl != null)
            {
                // check if instance have parent control
                if (this.parent != null)
                {
                    // check if parent control is equals to up control (first child)
                    if (this.parent.Equals(this.upControl))
                    {
                        // set up control as last control in line
                        lastControlInLine = this.upControl;
                    }
                    else
                    {
                        // up control is not equals to parent
                        lastControlInLine = this.upControl.GetLastControlInLine();
                    }
                }
                else
                {
                    // instance dont have parent control so up control is not equals as up control
                    lastControlInLine = this.upControl.GetLastControlInLine();
                }

                /// Zasto sam ovo stavljao ?????
                //if (lastControlInLine.TopParent.Equals(this.TopParent))
                //{
                //    lastControlInLine = this.upControl;
                //}


                if (lastControlInLine.Equals(this))
                {
                    lastControlInLine = this.upControl;
                }
                this.Top = lastControlInLine.Top + lastControlInLine.Height + this.distanceBetweenControl;
                if (this.parent == null)
                {
                    this.Left = this.upControl.Left;
                }
                else
                {
                    this.Left = this.parent.Left + this.childLeftOfSet;

                    //if (this.upControl.Equals(this.parent))
                    //{
                    //    this.Left = this.upControl.Left + this.childLeftOfSet;
                    //}
                    //else
                    //{
                    //    this.Left = this.upControl.Left;
                    //}
                }

                // check if child list control is not null
                if (childList != null)
                {
                    // move first child control, other child will do cascade efect
                    foreach (UserControlBase child in childList)
                    {
                        child.AlignSelf();
                    }
                    //firsChild = (UserControlBase)childList[0];
                    //firsChild.Top = this.Top + this.Height + this.distanceBetweenControl;
                    //firsChild.Left = this.Left + this.childLeftOfSet;
                    //firsChild.AlignSelf();
                    //firsChild.OnMove(null);
                }


            }
            else
            {
                this.Top = this.defaultTop;
                this.Left = this.defaultLeft;
            }

            // Align down control
            if (this.downControl != null)
            {
                this.downControl.AlignSelf();
            }
        }
        /// <summary>
        /// Executes remove of himself and all his children
        /// </summary>
        public virtual void Remove()
        {
            // call Remve event if any subscribers
            if (this.OnRemove != null)
            {
                this.OnRemove(this);
            }

            if (this.HasChildrenControls)
            {
                for (int i = childList.Count - 1; i >= 0; i--)
                {
                    ((UserControlBase)childList[i]).Remove();
                }
            }


            // if control have parent
            if (this.parent != null)
            {
                // remove seld from parent child list
                this.parent.childList.Remove(this);
            }

            // if control have upControl
            if (this.upControl != null)
            {
                // if upControl from this instace have downControl
                // TODO: ova provjera nema smisla jer upControl mora imati down control ako nema onda je negdje u prcesu nastala greska
                if (this.upControl.downControl != null)
                {
                    // now check if downControl is this instance
                    if (this.upControl.downControl.Equals(this))
                    {
                        // check if this instance have downControl
                        if (this.downControl != null)
                        {
                            // now replace downControl from upControl from this instance downControl
                            // this.downControl.upControl will be set automaticli in DownControl property
                            this.upControl.DownControl = this.downControl;
                            // now align downControl
                            this.upControl.DownControl.AlignSelf();

                        }
                        else
                        {
                            // this instance dont have downControl so just set downControl from upContro to null
                            this.upControl.DownControl = null;
                        }
                    }
                    else
                    {
                        // this is first child of upControl

                        // check if this instance have downControl (second in line child)
                        if (this.downControl != null)
                        {
                            this.downControl.upControl = this.upControl;
                            this.downControl.AlignSelf();
                        }
                    }
                }
                else
                {
                    // now check if this control hase parent
                    if (this.parent != null)
                    {
                        // check if parent hase downControl
                        if (this.parent.downControl != null)
                        {
                            this.parent.downControl.AlignSelf();
                        }
                    }

                    // check if this contol hase top parent
                    if (this.TopParent != null)
                    {
                        // check if top parent hase downControl
                        if (this.TopParent.downControl != null)
                        {
                            this.TopParent.downControl.AlignSelf();
                        }
                    }




                }
            }
            else
            {

                if (this.downControl != null)
                {
                    this.downControl.UpControl = null;
                    this.downControl.AlignSelf();
                    if (this.upControl != null)
                    {
                        this.downControl.defaultLeft = this.upControl.defaultLeft;
                        this.downControl.defaultTop = this.upControl.defaultTop; ;
                    }
                }
            }
            // if this instance have parent
            if (this.parent != null)
            {
                // if parent have downControl
                if (this.parent.downControl != null)
                {
                    // then align downControl
                    this.parent.downControl.AlignSelf();
                }
            }



            this.Dispose();

        }

        /// <summary>
        /// Get curent active child instance
        /// </summary>
        /// <returns>Instance or null</returns>
        public UserControlBase FetchCurentChild()
        {
            UserControlBase result = null;

            if (this.fetchNextChildCounter < this.childList.Count)
            {
                result = (UserControlBase)childList[this.fetchNextChildCounter];
            }

            return result;
        }
        /// <summary>
        /// Fetch next child instance or return null if child list is on end
        /// </summary>
        /// <returns>Instance or null</returns>
        public UserControlBase FetchNextChild()
        {
            UserControlBase result = null;

            if (this.fetchNextChildCounter + 1 < this.childList.Count)
            {
                this.fetchNextChildCounter++;
                result = (UserControlBase)childList[this.fetchNextChildCounter];
            }

            return result;
        }
        /// <summary>
        /// Peek next child instance or return null if child list is on end
        /// </summary>
        /// <returns>Instance or null</returns>
        public UserControlBase PeekNextChild()
        {
            UserControlBase result = null;

            if (this.fetchNextChildCounter + 1 < this.childList.Count)
            {
                result = (UserControlBase)childList[this.fetchNextChildCounter + 1];
            }

            return result;
        }

        /// <summary>
        /// Set FetchCurentChild() to first child in list
        /// </summary>
        public void ResetNextChildStatus()
        {
            this.fetchNextChildCounter = 0;
        }

        IEnumerator<UserControlBase> IEnumerable<UserControlBase>.GetEnumerator()
        {
            if (childList != null)
            {
                foreach (UserControlBase child in childList)
                {
                    yield return child;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // dummy method
            return null;
        }

        /// <summary>
        /// Za sad beskorisno jer se cast ne moze izvrsiti !!!
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="upControl"></param>
        /// <param name="downControl"></param>
        /// <returns></returns>
        public static Control GetControlInstance(Panel panel, Control upControl, Control downControl)
        {
            //UserControlBase control;
            Control control;
            control = new Control();

            control.Location = new System.Drawing.Point(10, 20);
            //rowCollection.Name = "Data" + columnCount;
            control.Size = new System.Drawing.Size(800, 30);
            control.TabIndex = 0;

            //control.UpControl = upControl;
            //control.DownControl = downControl;
            panel.Controls.Add(control);
            return control;
        } 
        #endregion


        #region ControlPropertys
        public UserControlBase UpControl
        {
            get { return this.upControl; }
            set
            {
                this.upControl = value;
                Control lastControlInLine = null;

                if (this.upControl != null)
                {
                    // Set default top and left values from up control
                    this.defaultTop = this.upControl.defaultTop;
                    this.defaultLeft = this.upControl.defaultLeft;
                    //
                    lastControlInLine = upControl.GetLastControlInLine();
                    this.upControl.downControl = this;
                    this.AlignSelf();
                }
            }
        }
        public UserControlBase DownControl
        {
            get { return this.downControl; }
            set
            {
                this.downControl = value;
                if (this.downControl != null)
                {
                    this.downControl.upControl = this;
                }
            }
        }
        public UserControlBase ParentControl
        {
            get
            {
                return this.parent;
            }
            set
            {
                // check if this instance has previous parent
                if (this.parent != null)
                {
                    // remove this instance from previous parent list
                    this.parent.childList.Remove(this);
                }

                // set new parent
                this.parent = value;

                // chech if parent is not null
                if (this.parent != null)
                {
                    // add self to parent list
                    this.parent.childList.Add(this);
                }
            }
        }
        public UserControlBase TopParent
        {
            get
            {
                UserControlBase topParent = this; ;
                do
                {
                    if (topParent.parent == null)
                    {
                        break;
                    }
                    topParent = topParent.parent;
                } while (true);

                return topParent;
            }
        }
        /// <summary>
        /// Return true if this control have logical children controls
        /// </summary>
        public bool HasChildrenControls
        {
            get
            {
                if (childList != null)
                {
                    if (childList.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Zero base index that represend depth in child relation
        /// </summary>
        public int DepthIndex
        {
            get
            {
                int depthIndex = 0;
                UserControlBase userControlBase = this;
                do
                {
                    if (userControlBase.parent != null)
                    {
                        depthIndex++;
                        userControlBase = userControlBase.parent;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                return depthIndex;
            }
        } 
        #endregion


    }
}
