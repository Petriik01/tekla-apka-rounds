using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using Tekla.Structures.Model;
using System.Threading;
using Tekla.Structures.Ui.WpfKit.Domain.Ribbon;
using System.Windows.Threading;
using System.Runtime.CompilerServices;

namespace apka
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TSM.Model modelSnake = new TSM.Model();


        TSM.Beam beamSnakeUp = new Beam(TSM.Beam.BeamTypeEnum.BEAM);
        TSM.Beam beamSnakeDown = new Beam(TSM.Beam.BeamTypeEnum.BEAM);
        TSM.Beam beamSnakeLeft = new Beam(TSM.Beam.BeamTypeEnum.BEAM);
        TSM.Beam beamSnakeRight = new Beam(TSM.Beam.BeamTypeEnum.BEAM);

        TSM.Beam beamBod = new Beam(TSM.Beam.BeamTypeEnum.BEAM);

        public MainWindow()
        {
            InitializeComponent();
        }
        // DORT
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double pointX;
            double pointXEnd;

            double pointOhenX;
            double pointOhenXEnd;

            double pointY;
            double pointYEnd;

            double pointOhenY;
            double pointOhenYEnd;

            double vyskaPatra = Convert.ToDouble(textBoxPatro.Text);
            string vyskaPatraString = textBoxPatro.Text;

            //svičky dokola
            double pocetSvicek = Convert.ToDouble(textBoxPocetSvicek.Text);

            int pocetVrstev = Convert.ToInt32(textBoxPocetVrstev.Text);

            string px, py, pz;
            px = textBoxPoziceX.Text;
            py = textBoxPoziceY.Text;
            pz = textBoxPoziceZ.Text;

            if (px == string.Empty)
            {
                px = "0";
            }
            if (py == string.Empty)
            {
                py = "0";
            }
            if (pz == string.Empty)
            {
                pz = "0";
            }

            double poziceX = Convert.ToDouble(px);
            double poziceY = Convert.ToDouble(py);
            double poziceZ = Convert.ToDouble(pz);

            if (poziceX < 0 || poziceY < 0 || poziceZ < 0)
            {
                MessageBox.Show("Jedna ze zadaných hodnot je menší jak 0.", "Nelze program dokončit");
                return;
            }



            TSM.Model model = new TSM.Model();

            if (model.GetConnectionStatus())
            {
                TSM.Beam beam1 = new TSM.Beam(TSM.Beam.BeamTypeEnum.BEAM);
                //COLUMN

                beam1.Profile.ProfileString = "200*200";
                beam1.Material.MaterialString = "C45/55";
                beam1.Class = "7";

                beam1.StartPoint = new TSG.Point(1000.0, 1000.0, 0.0);
                beam1.EndPoint = new TSG.Point(2000.0, 2000.0, 0.0);

                beam1.Insert();

                double bod1 = 2000;
                double bodMain = bod1;
                double bod2 = 0;
                double bod3 = 0;

                int colorClass = 1;



                double prumerDortu = bodMain;
                double polomerDortu = bodMain / 2;

                if ((bool)checkBoxRounded.IsChecked)
                {


                    List<TSG.Point> poziceSvicek = new List<TSG.Point>();

                    for (int i = 0; i < pocetSvicek; i++)
                    {
                        double uhel = (2 * Math.PI * i) / pocetSvicek;
                        double x = polomerDortu * Math.Cos(uhel) + polomerDortu;
                        double y = polomerDortu * Math.Sin(uhel) + polomerDortu;

                        TSG.Point p = new TSG.Point(x, y, vyskaPatra);
                        poziceSvicek.Add(p);
                    }

                    foreach (TSG.Point pozice in poziceSvicek)
                    {
                        TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                        svicka.Profile.ProfileString = "25*25";
                        svicka.Material.MaterialString = "C45/55";
                        svicka.Class = "7";
                        svicka.StartPoint = pozice;
                        svicka.EndPoint = new TSG.Point(pozice.X, pozice.Y, vyskaPatra + 100);
                        svicka.Insert();
                    }
                }
                else
                {
                    vyskaPatra = vyskaPatra - vyskaPatra / 2;
                    pointX = 100;
                    pointY = 100;
                    if (pocetSvicek > 36)
                    {
                        MessageBox.Show("zadaný počet svíček je příliš velký, maximum je 36", "Bylo přidáno pouze 36 svíček");
                    }

                    if (pocetSvicek > 10)
                    {
                        pocetSvicek -= 10;

                        for (int i = 0; i < 10; i++)
                        {
                            TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                            svicka.Profile.ProfileString = "25*25";
                            svicka.Material.MaterialString = "C45/55";
                            svicka.Class = "7";
                            svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                            svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                            svicka.Insert();

                            pointX += 200;
                        }

                        if (pocetSvicek > 9)
                        {
                            pocetSvicek -= 9;
                            pointX -= 200;
                            //
                            pointY += 200;
                            for (int i = 0; i < 9; i++)
                            {
                                TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                                svicka.Profile.ProfileString = "25*25";
                                svicka.Material.MaterialString = "C45/55";
                                svicka.Class = "7";
                                svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                                svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                                svicka.Insert();

                                pointY += 200;
                            }

                            if(pocetSvicek > 9)
                            {
                                pocetSvicek -= 9;
                                pointY -= 200;
                                //
                                pointX -= 200;
                                for (int i = 0; i < 9; i++)
                                {
                                    TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                                    svicka.Profile.ProfileString = "25*25";
                                    svicka.Material.MaterialString = "C45/55";
                                    svicka.Class = "7";
                                    svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                                    svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                                    svicka.Insert();

                                    pointX -= 200;
                                }
                                if (pocetSvicek > 0)
                                {
                                    //
                                    pointY -= 200;
                                    pointX += 200;
                                    for (int i = 0; i < pocetSvicek; i++)
                                    {
                                        TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                                        svicka.Profile.ProfileString = "25*25";
                                        svicka.Material.MaterialString = "C45/55";
                                        svicka.Class = "7";
                                        svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                                        svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                                        svicka.Insert();

                                        pointY -= 200;
                                    }
                                }
                            }
                            else
                            {
                                //
                                pointX -= 200;
                                pointY -= 200;
                                for (int i = 0; i < pocetSvicek; i++)
                                {
                                    TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                                    svicka.Profile.ProfileString = "25*25";
                                    svicka.Material.MaterialString = "C45/55";
                                    svicka.Class = "7";
                                    svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                                    svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                                    svicka.Insert();

                                    pointX -= 200;
                                }
                            }


                        }
                        else
                        {
                            pointY += 200;
                            pointX -= 200;
                            for (int i = 0; i < pocetSvicek; i++)
                            {
                                TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                                svicka.Profile.ProfileString = "25*25";
                                svicka.Material.MaterialString = "C45/55";
                                svicka.Class = "7";
                                svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                                svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                                svicka.Insert();

                                pointY += 200;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pocetSvicek; i++)
                        {
                            TSM.Beam svicka = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);
                            svicka.Profile.ProfileString = "25*25";
                            svicka.Material.MaterialString = "C45/55";
                            svicka.Class = "7";
                            svicka.StartPoint = new TSG.Point(pointX, pointY, vyskaPatra);
                            svicka.EndPoint = new TSG.Point(pointX, pointY, vyskaPatra + 100);
                            svicka.Insert();

                            pointX += 200;
                        }
                    }
                    
                    
                }
                // vytvoření ploch
                for (int i = 1; i <= pocetVrstev; i++)
                {
                    TSM.ContourPlate contourPlate1 = new TSM.ContourPlate();
                    contourPlate1.Profile.ProfileString = "PL" + vyskaPatraString;
                    contourPlate1.Material.MaterialString = "S235JR";
                    contourPlate1.Class = Convert.ToString(colorClass);

                    //CHAMFER (ořezání kruhu)
                    TSM.Chamfer chamfer = new Chamfer();

                    if ((bool)checkBoxRounded.IsChecked){
                        chamfer.Type = Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT;
                    }
                    else
                    {
                        chamfer.Type = Chamfer.ChamferTypeEnum.CHAMFER_NONE;
                    }
                    
                    contourPlate1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(bod1, bod1, bod3), chamfer));
                    contourPlate1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(bod1, bod2, bod3), chamfer));
                    contourPlate1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(bod2, bod2, bod3), chamfer));
                    contourPlate1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(bod2, bod1, bod3), chamfer));

                    contourPlate1.Insert();

                    bod1 = bod1 - 200;
                    bod2 = bod2 + 200;
                    bod3 = bod3 + vyskaPatra;

                    if (bod1 <= bod2)
                    {
                        MessageBox.Show("Maximální počet pater, které šlo vytvořit bylo " + i + " více už se na dort bohužel nevejde");
                        break;
                    }

                    colorClass++;
                }

                bod3 = bod3 - (2 * vyskaPatra);

                // VYTVOŘENÍ HLAVNÍ SVÍČKY ------------

                TSM.Beam beam2 = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);

                beam2.Profile.ProfileString = "50*50";
                beam2.Material.MaterialString = "C45/55";
                beam2.Class = "7";

                double endPoint = bod3 + 400;

                beam2.StartPoint = new TSG.Point(bodMain / 2 - 25.0, bodMain / 2, bod3);
                beam2.EndPoint = new TSG.Point(bodMain / 2 - 25.0, bodMain / 2, endPoint);

                beam2.Insert();

                // HLAVNÍ SVÍČKA KONEC ----------------

                // VYTVOŘENÍ HLAVNÍHO OHNĚ ------------

                TSM.Beam beam3 = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);

                beam3.Profile.ProfileString = "25*25";
                beam3.Material.MaterialString = "C45/55";
                beam3.Class = "13";

                beam3.StartPoint = new TSG.Point(bodMain / 2 + 12.5, bodMain / 2 - 12.5, endPoint);
                beam3.EndPoint = new TSG.Point(bodMain / 2 - 25.0, bodMain / 2 + 25.0, endPoint + 50);

                beam3.Insert();

                // HLAVNÍ OHEŇ KONEC ------------------
                model.CommitChanges();     
            }
        }

        //SNAKE

        public double hadX = 1000;
        public double hadXStart = 0;

        public double hadY = 0;
        public double hadYStart = 0;

        public double hadZ = 0;

        bool nahoru;
        bool dolu;
        bool doprava;
        bool doleva;

        int timeOut = 400;

        double posun = 500;

        public double pointX;
        public double pointY;

        public int pointCounter;

        private void buttonSnake_Click_1(object sender, RoutedEventArgs e)
        {
            beamSnakeUp.Profile.ProfileString = "150*150";
            beamSnakeUp.Material.MaterialString = "C45/55";
            beamSnakeUp.Class = "7";

            beamSnakeDown.Profile.ProfileString = "150*150";
            beamSnakeDown.Material.MaterialString = "C45/55";
            beamSnakeDown.Class = "7";

            beamSnakeLeft.Profile.ProfileString = "150*150";
            beamSnakeLeft.Material.MaterialString = "C45/55";
            beamSnakeLeft.Class = "7";

            beamSnakeRight.Profile.ProfileString = "150*150";
            beamSnakeRight.Material.MaterialString = "C45/55";
            beamSnakeRight.Class = "7";

            nahoru = true;

            generatePoint();

            new Thread(() =>
            {
                do
                {
                    beamSnakeUp.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                    beamSnakeUp.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                    beamSnakeUp.Insert();
                    modelSnake.CommitChanges();

                    beamSnakeUp.Delete();

                    hadXStart = hadX;
                    hadX += posun;

                    System.Threading.Thread.Sleep(timeOut);

                    if (hadX > 20000)
                    {
                        MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena | Prohrál jsi", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //Check jestli sebral Point
                    checkPoint();

                } while (nahoru == true);

            }).Start();
        }

        private void buttonSnakeStop_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();         
        }

        private void buttonSnakeUp_Click(object sender, RoutedEventArgs e)
        {
            buttonSnakeDown.IsEnabled = false;

            buttonSnakeLeft.IsEnabled = true;
            buttonSnakeRight.IsEnabled = true;

            if (dolu == true)
            {
                hadX += 2 * posun;
            }
            else
            {
                hadX += posun;
            }

            nahoru = true;

            dolu = false;
            doleva = false;
            doprava = false;
        
            hadY = hadYStart;

            new Thread(() =>
            {
                do
                {
                    beamSnakeUp.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                    beamSnakeUp.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                    beamSnakeUp.Insert();
                    modelSnake.CommitChanges();

                    beamSnakeUp.Delete();

                    hadXStart = hadX - pointCounter * 50;
                    
                    hadX += posun;

                    System.Threading.Thread.Sleep(timeOut);

                    if (hadX > 20000)
                    {
                        MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    //Check jestli sebral Point
                    checkPoint();

                } while (nahoru == true);
            }).Start();
        }

        private void buttonSnakeDown_Click(object sender, RoutedEventArgs e)
        {
            buttonSnakeUp.IsEnabled = false;

            buttonSnakeLeft.IsEnabled = true;
            buttonSnakeRight.IsEnabled = true;

            if (nahoru == true)
            {
                hadX -= 2 * posun;
            }
            else
            {
                hadX -= posun;
            }

            dolu = true;

            nahoru = false;
            doleva = false;
            doprava = false;

            hadY = hadYStart;

            new Thread(() =>
            {
                do
                {

                    beamSnakeDown.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                    beamSnakeDown.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                    beamSnakeDown.Insert();
                    modelSnake.CommitChanges();

                    beamSnakeDown.Delete();

                    hadXStart = hadX + pointCounter * 50;

                    hadX -= posun;

                    System.Threading.Thread.Sleep(timeOut);

                    if (hadX < 0)
                    {
                        MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    //Check jestli sebral Point
                    checkPoint();

                } while (dolu == true);
            }).Start();
        }

        private void buttonSnakeLeft_Click(object sender, RoutedEventArgs e)
        {
            buttonSnakeRight.IsEnabled = false;

            buttonSnakeUp.IsEnabled = true;
            buttonSnakeDown.IsEnabled = true;

            if (doprava == true)
            {
                hadY += 2 * posun;
            }
            else
            {
                hadY += posun;
            }

            doleva = true;

            nahoru = false;
            dolu = false;
            doprava = false;
           
            hadX = hadXStart;

            new Thread(() =>
            {
                do
                {
                    
                    beamSnakeLeft.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                    beamSnakeLeft.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                    beamSnakeLeft.Insert();
                    modelSnake.CommitChanges();

                    beamSnakeLeft.Delete();

                    hadYStart = hadY - pointCounter * 50;
                    
                    hadY += posun;

                    System.Threading.Thread.Sleep(timeOut);

                    if (hadY > 20000)
                    {
                        MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    //Check jestli sebral Point
                    checkPoint();

                } while (doleva == true);
            }).Start();
        }

        private void buttonSnakeRight_Click(object sender, RoutedEventArgs e)
        {
            buttonSnakeLeft.IsEnabled = false;

            buttonSnakeUp.IsEnabled = true;
            buttonSnakeDown.IsEnabled = true;

            if (doleva == true)
            {
                hadY -= 2 * posun;
            }
            else
            {
                hadY -= posun;
            }

            doprava = true;

            doleva = false;
            nahoru = false;
            dolu = false;


            hadX = hadXStart;

            new Thread(() => 
            {
                do
                {
                    
                    beamSnakeRight.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                    beamSnakeRight.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                    beamSnakeRight.Insert();
                    modelSnake.CommitChanges();

                    beamSnakeRight.Delete();

                    hadYStart = hadY + pointCounter * 50;

                    hadY -= posun;

                    System.Threading.Thread.Sleep(timeOut);

                    if (hadY < 0)
                    {
                        MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                    //Check jestli sebral Point
                    checkPoint();

                } while (doprava == true);

            }).Start();
        }

        //Checker jestli had sebral point
        private void checkPoint()
        {
            double rozdil1;
            double rozdil2;

            rozdil1 = Math.Abs(pointX - hadX);
            rozdil2 = Math.Abs(pointY - hadY);

            if (rozdil1 <= 250 && rozdil2 <= 250)
            {
                deletePoint();
            }
        }
        private void generatePoint()
        {
            beamBod.Profile.ProfileString = "250*250";
            beamBod.Material.MaterialString = "C45/55";
            beamBod.Class = "2";

            Random rndX = new Random();
            Random rndY = new Random();


            int ppX = rndX.Next(0, 20000);
            int ppY = rndY.Next(0, 20000);

            pointX = (int)Math.Round((double)ppX / 500) * 500;
            pointY = (int)Math.Round((double)ppY / 500) * 500;


            beamBod.StartPoint = new TSG.Point(pointX - 250 / 2, pointY, 250 / 2);
            beamBod.EndPoint = new TSG.Point(pointX + 250 / 2, pointY, 250 / 2);

            beamBod.Insert();
            modelSnake.CommitChanges();
        }

        private void deletePoint()
        {
            beamBod.Delete();

            pointCounter++;

            if (timeOut < 50)
            {
                timeOut = 50;
            }
            else
            {
                timeOut -= 5;
            }

            this.textBoxPoints.Dispatcher.Invoke(() =>
            {
                this.textBoxPoints.Text = Convert.ToString(pointCounter);
            });

            generatePoint();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            generatePoint();
        }
        // Ovládání pomocí kláves WASD
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D:
                    buttonSnakeLeft.IsEnabled = false;

                    buttonSnakeUp.IsEnabled = true;
                    buttonSnakeDown.IsEnabled = true;

                    if (doleva == true)
                    {
                        hadY -= 2 * posun;
                    }
                    else
                    {
                        hadY -= posun;
                    }

                    doprava = true;

                    doleva = false;
                    nahoru = false;
                    dolu = false;

                    hadX = hadXStart;

                    new Thread(() =>
                    {
                        do
                        {
                            beamSnakeRight.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                            beamSnakeRight.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                            beamSnakeRight.Insert();
                            modelSnake.CommitChanges();

                            beamSnakeRight.Delete();

                            hadYStart = hadY;
                            hadY -= posun;

                            System.Threading.Thread.Sleep(timeOut);

                            if (hadY < 0)
                            {
                                MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            }
                            //Check jestli sebral Point
                            checkPoint();
                        } while (doprava == true);

                    }).Start();
                    break;
                case Key.A:
                    buttonSnakeRight.IsEnabled = false;

                    buttonSnakeUp.IsEnabled = true;
                    buttonSnakeDown.IsEnabled = true;

                    if (doprava == true)
                    {
                        hadY += 2 * posun;
                    }
                    else
                    {
                        hadY += posun;
                    }

                    doleva = true;

                    nahoru = false;
                    dolu = false;
                    doprava = false;

                    hadX = hadXStart;

                    new Thread(() =>
                    {
                        do
                        {

                            beamSnakeLeft.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                            beamSnakeLeft.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                            beamSnakeLeft.Insert();
                            modelSnake.CommitChanges();

                            beamSnakeLeft.Delete();

                            hadYStart = hadY;
                            hadY += posun;

                            System.Threading.Thread.Sleep(timeOut);

                            if (hadY > 20000)
                            {
                                MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            }
                            //Check jestli sebral Point
                            checkPoint();

                        } while (doleva == true);
                    }).Start();
                    break;
                case Key.W:
                    buttonSnakeDown.IsEnabled = false;

                    buttonSnakeLeft.IsEnabled = true;
                    buttonSnakeRight.IsEnabled = true;

                    if (dolu == true)
                    {
                        hadX += 2 * posun;
                    }
                    else
                    {
                        hadX += posun;
                    }

                    nahoru = true;

                    dolu = false;
                    doleva = false;
                    doprava = false;

                    hadY = hadYStart;

                    new Thread(() =>
                    {
                        do
                        {
                            beamSnakeUp.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                            beamSnakeUp.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                            beamSnakeUp.Insert();
                            modelSnake.CommitChanges();

                            beamSnakeUp.Delete();

                            hadXStart = hadX;
                            hadX += posun;

                            System.Threading.Thread.Sleep(timeOut);

                            if (hadX > 20000)
                            {
                                MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            }
                            //Check jestli sebral Point
                            checkPoint();

                        } while (nahoru == true);
                    }).Start();
                    break;
                case Key.S:
                    buttonSnakeUp.IsEnabled = false;

                    buttonSnakeLeft.IsEnabled = true;
                    buttonSnakeRight.IsEnabled = true;

                    if (nahoru == true)
                    {
                        hadX -= 2 * posun;
                    }
                    else
                    {
                        hadX -= posun;
                    }

                    dolu = true;

                    nahoru = false;
                    doleva = false;
                    doprava = false;

                    hadY = hadYStart;

                    new Thread(() =>
                    {
                        do
                        {
                            beamSnakeDown.StartPoint = new TSG.Point(hadXStart, hadYStart, hadZ);
                            beamSnakeDown.EndPoint = new TSG.Point(hadX, hadY, hadZ);

                            beamSnakeDown.Insert();
                            modelSnake.CommitChanges();

                            beamSnakeDown.Delete();

                            hadXStart = hadX;
                            hadX -= posun;

                            System.Threading.Thread.Sleep(timeOut);

                            if (hadX < 0)
                            {
                                MessageBox.Show("Dostal jsi se mimo hrací pole", "Hra byla ukončena", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            }
                            //Check jestli sebral Point
                            checkPoint();

                        } while (dolu == true);
                    }).Start();
                    break;
            }
        }
    }
}
