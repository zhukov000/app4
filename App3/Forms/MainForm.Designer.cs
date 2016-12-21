using App3.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace App3.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
            ChartArea chartArea = new ChartArea();
            Series series = new Series();
            this.menuStrip = new MenuStrip();
            this.файлToolStripMenuItem = new ToolStripMenuItem();
            this.синхронизацияToolStripMenuItem = new ToolStripMenuItem();
            this.logToolStripMenuItem = new ToolStripMenuItem();
            this.фиксацияЖурналаToolStripMenuItem = new ToolStripMenuItem();
            this.выходToolStripMenuItem = new ToolStripMenuItem();
            this.справочникиToolStripMenuItem = new ToolStripMenuItem();
            this.районыToolStripMenuItem = new ToolStripMenuItem();
            this.районыToolStripMenuItem1 = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.объектыToolStripMenuItem = new ToolStripMenuItem();
            this.заказчикиToolStripMenuItem = new ToolStripMenuItem();
            this.организацииToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.типыКонтактовToolStripMenuItem = new ToolStripMenuItem();
            this.группыСообщенийToolStripMenuItem = new ToolStripMenuItem();
            this.иерархическийКлассификаторToolStripMenuItem = new ToolStripMenuItem();
            this.абонентскиеКомплектыToolStripMenuItem = new ToolStripMenuItem();
            this.addToolStripMenuItem = new ToolStripMenuItem();
            this.searchToolStripMenuItem = new ToolStripMenuItem();
            this.списокОбъектовToolStripMenuItem = new ToolStripMenuItem();
            this.принятыеДанныеToolStripMenuItem = new ToolStripMenuItem();
            this.событияToolStripMenuItem = new ToolStripMenuItem();
            this.событийOnlineToolStripMenuItem = new ToolStripMenuItem();
            this.графикСобытийToolStripMenuItem = new ToolStripMenuItem();
            this.отчетToolStripMenuItem = new ToolStripMenuItem();
            this.архивацияСобытийToolStripMenuItem = new ToolStripMenuItem();
            this.toolsMenu = new ToolStripMenuItem();
            this.optionsToolStripMenuItem = new ToolStripMenuItem();
            this.очиститьКэшToolStripMenuItem = new ToolStripMenuItem();
            this.warnToolStrip = new ToolStripMenuItem();
            this.mapToolStrip = new ToolStripMenuItem();
            this.soundToolStrip = new ToolStripMenuItem();
            this.окнаToolStripMenuItem = new ToolStripMenuItem();
            this.входToolStripMenuItem = new ToolStripMenuItem();
            this.toolStrip = new ToolStrip();
            this.toolStripButton1 = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.toolStripButton2 = new ToolStripButton();
            this.toolStripButton7 = new ToolStripButton();
            this.toolStripButton4 = new ToolStripButton();
            this.toolStripButton5 = new ToolStripButton();
            this.toolStripButton6 = new ToolStripButton();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.toolStripButton3 = new ToolStripButton();
            this.toolStripButton8 = new ToolStripButton();
            this.toolStripButton9 = new ToolStripButton();
            this.statusStrip = new StatusStrip();
            this.toolStripStatusLabel = new ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.toolStripProgressBar1 = new ToolStripProgressBar();
            this.toolTip = new ToolTip(this.components);
            this.backgroundWorker1 = new BackgroundWorker();
            this.panel1 = new Panel();
            this.chart1 = new Chart();
            this.panel2 = new Panel();
            this.label1 = new Label();
            this.label5 = new Label();
            this.ConnectCnt = new Label();
            this.label4 = new Label();
            this.label2 = new Label();
            this.timer1 = new Timer(this.components);
            this.journalToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((ISupportInitialize)this.chart1).BeginInit();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.menuStrip.Items.AddRange(new ToolStripItem[]
            {
                this.файлToolStripMenuItem,
                this.справочникиToolStripMenuItem,
                this.абонентскиеКомплектыToolStripMenuItem,
                this.принятыеДанныеToolStripMenuItem,
                this.toolsMenu,
                this.окнаToolStripMenuItem,
                this.входToolStripMenuItem
            });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(632, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            this.файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.синхронизацияToolStripMenuItem,
                this.logToolStripMenuItem,
                this.journalToolStripMenuItem,
                this.фиксацияЖурналаToolStripMenuItem,
                this.выходToolStripMenuItem
            });
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            this.синхронизацияToolStripMenuItem.Name = "синхронизацияToolStripMenuItem";
            this.синхронизацияToolStripMenuItem.ShortcutKeys = (Keys)131155;
            this.синхронизацияToolStripMenuItem.Size = new Size(200, 22);
            this.синхронизацияToolStripMenuItem.Text = "Синхронизация";
            this.синхронизацияToolStripMenuItem.Click += new EventHandler(this.синхронизацияToolStripMenuItem_Click);
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new Size(200, 22);
            this.logToolStripMenuItem.Text = "Журнал событий";
            this.logToolStripMenuItem.Click += new EventHandler(this.logToolStripMenuItem_Click);
            this.фиксацияЖурналаToolStripMenuItem.Name = "фиксацияЖурналаToolStripMenuItem";
            this.фиксацияЖурналаToolStripMenuItem.Size = new Size(200, 22);
            this.фиксацияЖурналаToolStripMenuItem.Text = "Фиксация журнала";
            this.фиксацияЖурналаToolStripMenuItem.Click += new EventHandler(this.фиксацияЖурналаToolStripMenuItem_Click);
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.ShortcutKeys = (Keys)131153;
            this.выходToolStripMenuItem.Size = new Size(200, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new EventHandler(this.выходToolStripMenuItem_Click);
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.районыToolStripMenuItem,
                this.районыToolStripMenuItem1,
                this.toolStripSeparator1,
                this.объектыToolStripMenuItem,
                this.заказчикиToolStripMenuItem,
                this.организацииToolStripMenuItem,
                this.toolStripSeparator4,
                this.типыКонтактовToolStripMenuItem,
                this.группыСообщенийToolStripMenuItem,
                this.иерархическийКлассификаторToolStripMenuItem
            });
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new Size(94, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            this.районыToolStripMenuItem.Name = "районыToolStripMenuItem";
            this.районыToolStripMenuItem.Size = new Size(248, 22);
            this.районыToolStripMenuItem.Text = "IP-адреса районов";
            this.районыToolStripMenuItem.Click += new EventHandler(this.районыToolStripMenuItem_Click);
            this.районыToolStripMenuItem1.Name = "районыToolStripMenuItem1";
            this.районыToolStripMenuItem1.Size = new Size(248, 22);
            this.районыToolStripMenuItem1.Text = "Районы";
            this.районыToolStripMenuItem1.Click += new EventHandler(this.районыToolStripMenuItem1_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(245, 6);
            this.объектыToolStripMenuItem.Name = "объектыToolStripMenuItem";
            this.объектыToolStripMenuItem.Size = new Size(248, 22);
            this.объектыToolStripMenuItem.Text = "Объекты";
            this.объектыToolStripMenuItem.Click += new EventHandler(this.объектыToolStripMenuItem_Click);
            this.заказчикиToolStripMenuItem.Name = "заказчикиToolStripMenuItem";
            this.заказчикиToolStripMenuItem.Size = new Size(248, 22);
            this.заказчикиToolStripMenuItem.Text = "Заказчики";
            this.заказчикиToolStripMenuItem.Click += new EventHandler(this.заказчикиToolStripMenuItem_Click);
            this.организацииToolStripMenuItem.Name = "организацииToolStripMenuItem";
            this.организацииToolStripMenuItem.Size = new Size(248, 22);
            this.организацииToolStripMenuItem.Text = "Компании";
            this.организацииToolStripMenuItem.Click += new EventHandler(this.организацииToolStripMenuItem_Click);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new Size(245, 6);
            this.типыКонтактовToolStripMenuItem.Name = "типыКонтактовToolStripMenuItem";
            this.типыКонтактовToolStripMenuItem.Size = new Size(248, 22);
            this.типыКонтактовToolStripMenuItem.Text = "Типы контактов";
            this.типыКонтактовToolStripMenuItem.Click += new EventHandler(this.типыКонтактовToolStripMenuItem_Click);
            this.группыСообщенийToolStripMenuItem.Name = "группыСообщенийToolStripMenuItem";
            this.группыСообщенийToolStripMenuItem.Size = new Size(248, 22);
            this.группыСообщенийToolStripMenuItem.Text = "Группы сообщений";
            this.группыСообщенийToolStripMenuItem.Click += new EventHandler(this.группыСообщенийToolStripMenuItem_Click);
            this.иерархическийКлассификаторToolStripMenuItem.Name = "иерархическийКлассификаторToolStripMenuItem";
            this.иерархическийКлассификаторToolStripMenuItem.Size = new Size(248, 22);
            this.иерархическийКлассификаторToolStripMenuItem.Text = "Иерархический классификатор";
            this.иерархическийКлассификаторToolStripMenuItem.Click += new EventHandler(this.иерархическийКлассификаторToolStripMenuItem_Click);
            this.абонентскиеКомплектыToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.addToolStripMenuItem,
                this.searchToolStripMenuItem,
                this.списокОбъектовToolStripMenuItem
            });
            this.абонентскиеКомплектыToolStripMenuItem.Name = "абонентскиеКомплектыToolStripMenuItem";
            this.абонентскиеКомплектыToolStripMenuItem.Size = new Size(74, 20);
            this.абонентскиеКомплектыToolStripMenuItem.Text = "Картотека";
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = (Keys)131137;
            this.addToolStripMenuItem.Size = new Size(173, 22);
            this.addToolStripMenuItem.Text = "Добавить";
            this.addToolStripMenuItem.Click += new EventHandler(this.addToolStripMenuItem_Click);
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.ShortcutKeys = (Keys)131142;
            this.searchToolStripMenuItem.Size = new Size(173, 22);
            this.searchToolStripMenuItem.Text = "Поиск";
            this.searchToolStripMenuItem.Click += new EventHandler(this.searchToolStripMenuItem_Click);
            this.списокОбъектовToolStripMenuItem.Name = "списокОбъектовToolStripMenuItem";
            this.списокОбъектовToolStripMenuItem.ShortcutKeys = (Keys)131148;
            this.списокОбъектовToolStripMenuItem.Size = new Size(173, 22);
            this.списокОбъектовToolStripMenuItem.Text = "Список АК";
            this.списокОбъектовToolStripMenuItem.Click += new EventHandler(this.списокОбъектовToolStripMenuItem_Click);
            this.принятыеДанныеToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.событияToolStripMenuItem,
                this.событийOnlineToolStripMenuItem,
                this.графикСобытийToolStripMenuItem,
                this.отчетToolStripMenuItem,
                this.архивацияСобытийToolStripMenuItem
            });
            this.принятыеДанныеToolStripMenuItem.Name = "принятыеДанныеToolStripMenuItem";
            this.принятыеДанныеToolStripMenuItem.Size = new Size(119, 20);
            this.принятыеДанныеToolStripMenuItem.Text = "Принятые данные";
            this.событияToolStripMenuItem.Name = "событияToolStripMenuItem";
            this.событияToolStripMenuItem.ShortcutKeys = (Keys)131141;
            this.событияToolStripMenuItem.Size = new Size(198, 22);
            this.событияToolStripMenuItem.Text = "Архив событий";
            this.событияToolStripMenuItem.Click += new EventHandler(this.событияToolStripMenuItem_Click);
            this.событийOnlineToolStripMenuItem.Name = "событийOnlineToolStripMenuItem";
            this.событийOnlineToolStripMenuItem.ShortcutKeys = Keys.F12;
            this.событийOnlineToolStripMenuItem.Size = new Size(198, 22);
            this.событийOnlineToolStripMenuItem.Text = "Событий online";
            this.событийOnlineToolStripMenuItem.Click += new EventHandler(this.событийOnlineToolStripMenuItem_Click);
            this.графикСобытийToolStripMenuItem.Name = "графикСобытийToolStripMenuItem";
            this.графикСобытийToolStripMenuItem.ShortcutKeys = Keys.F11;
            this.графикСобытийToolStripMenuItem.Size = new Size(198, 22);
            this.графикСобытийToolStripMenuItem.Text = "График событий";
            this.графикСобытийToolStripMenuItem.Click += new EventHandler(this.графикСобытийToolStripMenuItem_Click);
            this.отчетToolStripMenuItem.Name = "отчетToolStripMenuItem";
            this.отчетToolStripMenuItem.ShortcutKeys = (Keys)131154;
            this.отчетToolStripMenuItem.Size = new Size(198, 22);
            this.отчетToolStripMenuItem.Text = "Отчет";
            this.отчетToolStripMenuItem.Click += new EventHandler(this.отчетToolStripMenuItem_Click);
            this.архивацияСобытийToolStripMenuItem.Name = "архивацияСобытийToolStripMenuItem";
            this.архивацияСобытийToolStripMenuItem.Size = new Size(198, 22);
            this.архивацияСобытийToolStripMenuItem.Text = "Архивация событий";
            this.архивацияСобытийToolStripMenuItem.Click += new EventHandler(this.архивацияСобытийToolStripMenuItem_Click);
            this.toolsMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.optionsToolStripMenuItem,
                this.очиститьКэшToolStripMenuItem,
                this.warnToolStrip,
                this.mapToolStrip,
                this.soundToolStrip
            });
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new Size(70, 20);
            this.toolsMenu.Text = "Свойства";
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = (Keys)131159;
            this.optionsToolStripMenuItem.Size = new Size(209, 22);
            this.optionsToolStripMenuItem.Text = "Настройки";
            this.optionsToolStripMenuItem.Click += new EventHandler(this.optionsToolStripMenuItem_Click);
            this.очиститьКэшToolStripMenuItem.Name = "очиститьКэшToolStripMenuItem";
            this.очиститьКэшToolStripMenuItem.Size = new Size(209, 22);
            this.очиститьКэшToolStripMenuItem.Text = "Очистить кэш";
            this.очиститьКэшToolStripMenuItem.Click += new EventHandler(this.очиститьКэшToolStripMenuItem_Click);
            this.warnToolStrip.CheckOnClick = true;
            this.warnToolStrip.Name = "warnToolStrip";
            this.warnToolStrip.Size = new Size(209, 22);
            this.warnToolStrip.Text = "Отключить оповещение";
            this.mapToolStrip.Checked = true;
            this.mapToolStrip.CheckOnClick = true;
            this.mapToolStrip.CheckState = CheckState.Checked;
            this.mapToolStrip.Name = "mapToolStrip";
            this.mapToolStrip.Size = new Size(209, 22);
            this.mapToolStrip.Text = "Карта";
            this.mapToolStrip.Click += new EventHandler(this.картаToolStripMenuItem_Click_1);
            this.soundToolStrip.Checked = true;
            this.soundToolStrip.CheckOnClick = true;
            this.soundToolStrip.CheckState = CheckState.Checked;
            this.soundToolStrip.Name = "soundToolStrip";
            this.soundToolStrip.Size = new Size(209, 22);
            this.soundToolStrip.Text = "Звуковое оповещение";
            this.soundToolStrip.Click += new EventHandler(this.soundTooItem_Click);
            this.окнаToolStripMenuItem.Name = "окнаToolStripMenuItem";
            this.окнаToolStripMenuItem.Size = new Size(47, 20);
            this.окнаToolStripMenuItem.Text = "Окна";
            this.окнаToolStripMenuItem.Visible = false;
            this.входToolStripMenuItem.Name = "входToolStripMenuItem";
            this.входToolStripMenuItem.ShortcutKeys = (Keys)131148;
            this.входToolStripMenuItem.Size = new Size(44, 20);
            this.входToolStripMenuItem.Text = "Вход";
            this.входToolStripMenuItem.Click += new EventHandler(this.входToolStripMenuItem_Click);
            this.toolStrip.Items.AddRange(new ToolStripItem[]
            {
                this.toolStripButton1,
                this.toolStripSeparator2,
                this.toolStripButton2,
                this.toolStripButton7,
                this.toolStripButton4,
                this.toolStripButton5,
                this.toolStripButton6,
                this.toolStripSeparator3,
                this.toolStripButton3,
                this.toolStripButton8,
                this.toolStripButton9
            });
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new Size(632, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = (Image)componentResourceManager.GetObject("toolStripButton1.Image");
            this.toolStripButton1.ImageTransparentColor = Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new Size(23, 22);
            this.toolStripButton1.Text = "Опции";
            this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 25);
            this.toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = (Image)componentResourceManager.GetObject("toolStripButton2.Image");
            this.toolStripButton2.ImageTransparentColor = Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new Size(23, 22);
            this.toolStripButton2.Text = "Таблица сообщений";
            this.toolStripButton2.Click += new EventHandler(this.toolStripButton2_Click);
            this.toolStripButton7.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = Resources.tworows_9707;
            this.toolStripButton7.ImageTransparentColor = Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new Size(23, 22);
            this.toolStripButton7.Text = "Активность";
            this.toolStripButton7.Click += new EventHandler(this.toolStripButton7_Click);
            this.toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.ImageTransparentColor = Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new Size(23, 22);
            this.toolStripButton4.Text = "Окно статистики";
            this.toolStripButton4.Visible = false;
            this.toolStripButton4.Click += new EventHandler(this.toolStripButton4_Click_1);
            this.toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = (Image)componentResourceManager.GetObject("toolStripButton5.Image");
            this.toolStripButton5.ImageTransparentColor = Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new Size(23, 22);
            this.toolStripButton5.Text = "Принятые пакеты";
            this.toolStripButton5.Click += new EventHandler(this.toolStripButton5_Click);
            this.toolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = Resources.ThresholdRulenode_8715_24;
            this.toolStripButton6.ImageTransparentColor = Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new Size(23, 22);
            this.toolStripButton6.Text = "Окно тревожных сообщений";
            this.toolStripButton6.Click += new EventHandler(this.toolStripButton6_Click_1);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new Size(6, 25);
            this.toolStripButton3.Alignment = ToolStripItemAlignment.Right;
            this.toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = Resources.close;
            this.toolStripButton3.ImageTransparentColor = Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new Size(23, 22);
            this.toolStripButton3.Text = "Закрыть";
            this.toolStripButton3.Click += new EventHandler(this.toolStripButton3_Click);
            this.toolStripButton8.Alignment = ToolStripItemAlignment.Right;
            this.toolStripButton8.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = Resources.maximize;
            this.toolStripButton8.ImageTransparentColor = Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new Size(23, 22);
            this.toolStripButton8.Text = "Окно на весь экран";
            this.toolStripButton8.Click += new EventHandler(this.toolStripButton8_Click);
            this.toolStripButton9.Alignment = ToolStripItemAlignment.Right;
            this.toolStripButton9.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = Resources.minimize;
            this.toolStripButton9.ImageTransparentColor = Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new Size(23, 22);
            this.toolStripButton9.Text = "Минимизировать";
            this.toolStripButton9.Click += new EventHandler(this.toolStripButton9_Click);
            this.statusStrip.BackColor = Color.White;
            this.statusStrip.Items.AddRange(new ToolStripItem[]
            {
                this.toolStripStatusLabel,
                this.toolStripStatusLabel1,
                this.toolStripProgressBar1
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(632, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(93, 17);
            this.toolStripStatusLabel1.Text = "Синхронизация";
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new Size(100, 16);
            this.toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            this.panel1.BackColor = SystemColors.Control;
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Font = new Font("Times New Roman", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 204);
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(632, 59);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            chartArea.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea);
            this.chart1.Dock = DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(200, 0);
            this.chart1.Name = "chart1";
            series.ChartArea = "ChartArea1";
            series.Name = "Series1";
            this.chart1.Series.Add(series);
            this.chart1.Size = new Size(432, 59);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.ConnectCnt);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(200, 59);
            this.panel2.TabIndex = 8;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(167, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество подключений";
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(161, 31);
            this.label5.Name = "label5";
            this.label5.Size = new Size(36, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "мин.";
            this.ConnectCnt.AutoSize = true;
            this.ConnectCnt.Location = new System.Drawing.Point(182, 10);
            this.ConnectCnt.Name = "ConnectCnt";
            this.ConnectCnt.Size = new Size(15, 17);
            this.ConnectCnt.TabIndex = 1;
            this.ConnectCnt.Text = "0";
            this.label4.Location = new System.Drawing.Point(108, 31);
            this.label4.Name = "label4";
            this.label4.Size = new Size(58, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "0";
            this.label4.TextAlign = ContentAlignment.MiddleRight;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 31);
            this.label2.Name = "label2";
            this.label2.Size = new Size(99, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время работы";
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.journalToolStripMenuItem.Name = "journalToolStripMenuItem";
            this.journalToolStripMenuItem.Size = new Size(200, 22);
            this.journalToolStripMenuItem.Text = "Журнал работы";
            this.journalToolStripMenuItem.Click += new EventHandler(this.журналРаботыToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.ClientSize = new Size(632, 453);
            base.ControlBox = false;
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.statusStrip);
            base.Controls.Add(this.toolStrip);
            base.Controls.Add(this.menuStrip);
            base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            base.IsMdiContainer = true;
            base.MainMenuStrip = this.menuStrip;
            base.Name = "MainForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ГМК Сполох";
            base.TransparencyKey = Color.DimGray;
            base.WindowState = FormWindowState.Maximized;
            base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
            base.Load += new EventHandler(this.MainForm_Load);
            base.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
            base.Resize += new EventHandler(this.MainForm_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize)this.chart1).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();

        }
        #endregion


        private MenuStrip menuStrip;
        private ToolStrip toolStrip;
        private StatusStrip statusStrip;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolStripMenuItem toolsMenu;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolTip toolTip;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripButton toolStripButton1;
        private ToolStripMenuItem принятыеДанныеToolStripMenuItem;
        private ToolStripMenuItem событияToolStripMenuItem;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem выходToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem абонентскиеКомплектыToolStripMenuItem;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripButton toolStripButton5;
        private ToolStripMenuItem справочникиToolStripMenuItem;
        private ToolStripMenuItem районыToolStripMenuItem;
        private ToolStripMenuItem районыToolStripMenuItem1;
        private ToolStripMenuItem типыКонтактовToolStripMenuItem;
        private ToolStripMenuItem очиститьКэшToolStripMenuItem;
        private Panel panel1;
        private ToolStripButton toolStripButton7;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Label ConnectCnt;
        private Label label1;
        private Timer timer1;
        private Label label5;
        private Label label4;
        private Label label2;
        private ToolStripMenuItem группыСообщенийToolStripMenuItem;
        private ToolStripButton toolStripButton4;
        private ToolStripMenuItem событийOnlineToolStripMenuItem;
        private ToolStripMenuItem графикСобытийToolStripMenuItem;
        private ToolStripMenuItem окнаToolStripMenuItem;
        private ToolStripMenuItem иерархическийКлассификаторToolStripMenuItem;
        private ToolStripMenuItem списокОбъектовToolStripMenuItem;
        private ToolStripButton toolStripButton6;
        private ToolStripMenuItem отчетToolStripMenuItem;
        private ToolStripMenuItem warnToolStrip;
        private ToolStripButton toolStripButton8;
        private ToolStripMenuItem объектыToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem заказчикиToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private Panel panel2;
        private ToolStripButton toolStripButton9;
        private ToolStripMenuItem организацииToolStripMenuItem;
        private ToolStripMenuItem mapToolStrip;
        private ToolStripMenuItem архивацияСобытийToolStripMenuItem;
        private ToolStripMenuItem soundToolStrip;
        private ToolStripMenuItem синхронизацияToolStripMenuItem;
        private ToolStripMenuItem входToolStripMenuItem;
        private ToolStripMenuItem фиксацияЖурналаToolStripMenuItem;

        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripMenuItem logToolStripMenuItem;
        private ToolStripMenuItem journalToolStripMenuItem;
    }
}



