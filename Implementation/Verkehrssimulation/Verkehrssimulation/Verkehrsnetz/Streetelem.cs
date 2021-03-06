﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Verkehrssimulation.Verkehrsnetz
{
    abstract class EnvElement
    {
        public enum StreetType { Street = 1, ThreeKreuzung = 2, FourKreuzung = 3, Grass = 4, greenCircle = 5 };
        protected Image img;
        protected int x, y, dir;
        protected StreetType stype;
        public Image getImage()
        {
            return img;
        }

        public void enter(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 0.9;
        }

        public void leave(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 1;
        }
    }

    class Streetelem : EnvElement
    {
        private int ampelid = 0;
        private int rotation = 1;

        public Streetelem(int _x, int _y, int _dir, int _type, int ampelid)
        {
            this.ampelid = ampelid;
            x = _x;
            y = _y;
            dir = _dir;
            stype = (StreetType)_type;
            // Create the image element.
            img = new Image();
            img.Width = 100;
            img.Height = 100;
            img.MouseEnter += enter;
            img.MouseLeave += leave;
            img.MouseDown += printStreetType;

            initImgType();

        }

        public int getAmpelID()
        {
            return ampelid;
        }

        public void updateType(StreetType type)
        {
            this.stype = type;
            initImgType();
        }

        private void initImgType()
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();


            switch (stype)
            {
                case StreetType.Grass:
                    bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/grass.bmp", UriKind.RelativeOrAbsolute);
                    break;
                case StreetType.Street:
                    bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/street.bmp", UriKind.RelativeOrAbsolute);
                    break;
                case StreetType.FourKreuzung:
                    bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/4kreuzung.bmp", UriKind.RelativeOrAbsolute);
                    break;
                case StreetType.ThreeKreuzung:
                    bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/3kreuzung.bmp", UriKind.RelativeOrAbsolute);
                    break;
                case StreetType.greenCircle:
                    bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/greenCircle.bmp", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/grass.bmp", UriKind.RelativeOrAbsolute);
                    break;      
            }
            
            bi.EndInit();
            img.Source = bi;

        }

        public StreetType getStreetType()
        {
            return this.stype;
        }
        public void printStreetType(object sender, EventArgs e)
        {

            Console.WriteLine("Ampelid: " + this.ampelid);
            Console.WriteLine("Position (X/Y) " + this.x + this.y);
            Console.WriteLine(this.stype.ToString());
        }
        
        public int getRotation()
        {
            return this.rotation;
        }


        //**** Experimentelle Methoden ***//

        /// <summary>
        /// optional -> Könnte weitergeführt werden um die Straße dynamisch veränderbar zu machen
        /// </summary>
        private void addContextMenue()
        {
            ContextMenu pMenu = new ContextMenu();
            MenuItem item1 = new MenuItem();
            MenuItem item2 = new MenuItem();
            MenuItem item3 = new MenuItem();
            MenuItem item4 = new MenuItem();
            MenuItem item5 = new MenuItem();
            MenuItem item6 = new MenuItem();

            item1.Header = "Rotate";
            item1.Click += new RoutedEventHandler(elemRotate);
            pMenu.Items.Add(item1);

            item2.Header = "Kreuzung";
            item2.Click += new RoutedEventHandler(changetoKreuzung);
            pMenu.Items.Add(item2);

            item3.Header = "3Kreuzung";
            item3.Click += new RoutedEventHandler(changeto3Kreuzung);
            pMenu.Items.Add(item3);

            item4.Header = "Straße";
            item4.Click += new RoutedEventHandler(changetoStreet);
            pMenu.Items.Add(item4);

            item5.Header = "Grass";
            item5.Click += new RoutedEventHandler(changetoGrass);
            pMenu.Items.Add(item5);

            item6.Header = "Hinderniss";
            item6.Click += new RoutedEventHandler(addObstacle);
            pMenu.Items.Add(item6);

            // img.ContextMenu = pMenu;
        }

        public void changetoKreuzung(object sender, EventArgs e)
        {

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/4kreuzung.bmp", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.stype = StreetType.FourKreuzung;
            img.Source = bi;

        }

        public void changeto3Kreuzung(object sender, EventArgs e)
        {

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/3kreuzung.bmp", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.stype = StreetType.ThreeKreuzung;
            img.Source = bi;

        }

        public void changetoStreet(object sender, EventArgs e)
        {

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/street.bmp", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.stype = StreetType.Street;
            img.Source = bi;

        }

        public void changetoGrass(object sender, EventArgs e)
        {

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/Verkehrsnetz/FieldBitmaps/grass.bmp", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            this.stype = StreetType.Grass;
            img.Source = bi;

        }

        public void addObstacle(object sender, EventArgs e) { }

        public void elemRotate(object sender, EventArgs e)
        {

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = (img.Source as BitmapImage).UriSource;
            // new Uri((img.Source as BitmapImage).UriSource.ToString(), UriKind.RelativeOrAbsolute);

            if ((img.Source as BitmapImage).Rotation == Rotation.Rotate0)
            {
                bi.Rotation = Rotation.Rotate90; //(img.Source as BitmapImage).Rotation;
            }
            else if ((img.Source as BitmapImage).Rotation == Rotation.Rotate90)
            {
                bi.Rotation = Rotation.Rotate180; //(img.Source as BitmapImage).Rotation;
            }
            else if ((img.Source as BitmapImage).Rotation == Rotation.Rotate180)
            {
                bi.Rotation = Rotation.Rotate270; //(img.Source as BitmapImage).Rotation;
            }
            else if ((img.Source as BitmapImage).Rotation == Rotation.Rotate270)
            {
                bi.Rotation = Rotation.Rotate0; //(img.Source as BitmapImage).Rotation;
            }

            bi.EndInit();

            img.Source = bi;
            this.rotation = 2;

        }

    }
}
