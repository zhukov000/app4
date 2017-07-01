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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.синхронизацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отправитьПоследниеСообщенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.journalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.фиксацияЖурналаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.районыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.районыToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.объектыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заказчикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.организацииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.типыКонтактовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.группыСообщенийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.иерархическийКлассификаторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.абонентскиеКомплектыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокОбъектовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.принятыеДанныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.событияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.событийOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.графикСобытийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.архивацияСобытийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьСтатусыРайоновToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.очиститьКэшToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьВесьКэшToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warnToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.soundToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.тестовоеСообщениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.мониторУзловToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.окнаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.входToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ConnectCnt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.синхронизацияОбъектовССервераToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.справочникиToolStripMenuItem,
            this.абонентскиеКомплектыToolStripMenuItem,
            this.принятыеДанныеToolStripMenuItem,
            this.toolsMenu,
            this.окнаToolStripMenuItem,
            this.входToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(632, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.синхронизацияToolStripMenuItem,
            this.синхронизацияОбъектовССервераToolStripMenuItem,
            this.отправитьПоследниеСообщенияToolStripMenuItem,
            this.logToolStripMenuItem,
            this.journalToolStripMenuItem,
            this.фиксацияЖурналаToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // синхронизацияToolStripMenuItem
            // 
            this.синхронизацияToolStripMenuItem.Name = "синхронизацияToolStripMenuItem";
            this.синхронизацияToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.синхронизацияToolStripMenuItem.Text = "Синхронизация";
            this.синхронизацияToolStripMenuItem.Click += new System.EventHandler(this.синхронизацияToolStripMenuItem_Click);
            // 
            // отправитьПоследниеСообщенияToolStripMenuItem
            // 
            this.отправитьПоследниеСообщенияToolStripMenuItem.Name = "отправитьПоследниеСообщенияToolStripMenuItem";
            this.отправитьПоследниеСообщенияToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.отправитьПоследниеСообщенияToolStripMenuItem.Text = "Синхронизация последних сообщения";
            this.отправитьПоследниеСообщенияToolStripMenuItem.Click += new System.EventHandler(this.отправитьПоследниеСообщенияToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.logToolStripMenuItem.Text = "Журнал событий";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // journalToolStripMenuItem
            // 
            this.journalToolStripMenuItem.Name = "journalToolStripMenuItem";
            this.journalToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.journalToolStripMenuItem.Text = "Журнал работы";
            this.journalToolStripMenuItem.Click += new System.EventHandler(this.журналРаботыToolStripMenuItem_Click);
            // 
            // фиксацияЖурналаToolStripMenuItem
            // 
            this.фиксацияЖурналаToolStripMenuItem.Name = "фиксацияЖурналаToolStripMenuItem";
            this.фиксацияЖурналаToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.фиксацияЖурналаToolStripMenuItem.Text = "Фиксация журнала";
            this.фиксацияЖурналаToolStripMenuItem.Click += new System.EventHandler(this.фиксацияЖурналаToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.районыToolStripMenuItem,
            this.районыToolStripMenuItem1,
            this.toolStripSeparator1,
            this.объектыToolStripMenuItem,
            this.заказчикиToolStripMenuItem,
            this.организацииToolStripMenuItem,
            this.toolStripSeparator4,
            this.типыКонтактовToolStripMenuItem,
            this.группыСообщенийToolStripMenuItem,
            this.иерархическийКлассификаторToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // районыToolStripMenuItem
            // 
            this.районыToolStripMenuItem.Name = "районыToolStripMenuItem";
            this.районыToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.районыToolStripMenuItem.Text = "IP-адреса районов";
            this.районыToolStripMenuItem.Click += new System.EventHandler(this.районыToolStripMenuItem_Click);
            // 
            // районыToolStripMenuItem1
            // 
            this.районыToolStripMenuItem1.Name = "районыToolStripMenuItem1";
            this.районыToolStripMenuItem1.Size = new System.Drawing.Size(248, 22);
            this.районыToolStripMenuItem1.Text = "Районы";
            this.районыToolStripMenuItem1.Click += new System.EventHandler(this.районыToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(245, 6);
            // 
            // объектыToolStripMenuItem
            // 
            this.объектыToolStripMenuItem.Name = "объектыToolStripMenuItem";
            this.объектыToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.объектыToolStripMenuItem.Text = "Объекты";
            this.объектыToolStripMenuItem.Click += new System.EventHandler(this.объектыToolStripMenuItem_Click);
            // 
            // заказчикиToolStripMenuItem
            // 
            this.заказчикиToolStripMenuItem.Name = "заказчикиToolStripMenuItem";
            this.заказчикиToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.заказчикиToolStripMenuItem.Text = "Заказчики";
            this.заказчикиToolStripMenuItem.Click += new System.EventHandler(this.заказчикиToolStripMenuItem_Click);
            // 
            // организацииToolStripMenuItem
            // 
            this.организацииToolStripMenuItem.Name = "организацииToolStripMenuItem";
            this.организацииToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.организацииToolStripMenuItem.Text = "Компании";
            this.организацииToolStripMenuItem.Click += new System.EventHandler(this.организацииToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(245, 6);
            // 
            // типыКонтактовToolStripMenuItem
            // 
            this.типыКонтактовToolStripMenuItem.Name = "типыКонтактовToolStripMenuItem";
            this.типыКонтактовToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.типыКонтактовToolStripMenuItem.Text = "Типы контактов";
            this.типыКонтактовToolStripMenuItem.Click += new System.EventHandler(this.типыКонтактовToolStripMenuItem_Click);
            // 
            // группыСообщенийToolStripMenuItem
            // 
            this.группыСообщенийToolStripMenuItem.Name = "группыСообщенийToolStripMenuItem";
            this.группыСообщенийToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.группыСообщенийToolStripMenuItem.Text = "Группы сообщений";
            this.группыСообщенийToolStripMenuItem.Click += new System.EventHandler(this.группыСообщенийToolStripMenuItem_Click);
            // 
            // иерархическийКлассификаторToolStripMenuItem
            // 
            this.иерархическийКлассификаторToolStripMenuItem.Name = "иерархическийКлассификаторToolStripMenuItem";
            this.иерархическийКлассификаторToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.иерархическийКлассификаторToolStripMenuItem.Text = "Иерархический классификатор";
            this.иерархическийКлассификаторToolStripMenuItem.Click += new System.EventHandler(this.иерархическийКлассификаторToolStripMenuItem_Click);
            // 
            // абонентскиеКомплектыToolStripMenuItem
            // 
            this.абонентскиеКомплектыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.списокОбъектовToolStripMenuItem});
            this.абонентскиеКомплектыToolStripMenuItem.Name = "абонентскиеКомплектыToolStripMenuItem";
            this.абонентскиеКомплектыToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.абонентскиеКомплектыToolStripMenuItem.Text = "Картотека";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.addToolStripMenuItem.Text = "Добавить";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.searchToolStripMenuItem.Text = "Поиск";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // списокОбъектовToolStripMenuItem
            // 
            this.списокОбъектовToolStripMenuItem.Name = "списокОбъектовToolStripMenuItem";
            this.списокОбъектовToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.списокОбъектовToolStripMenuItem.Text = "Список АК";
            this.списокОбъектовToolStripMenuItem.Click += new System.EventHandler(this.списокОбъектовToolStripMenuItem_Click);
            // 
            // принятыеДанныеToolStripMenuItem
            // 
            this.принятыеДанныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.событияToolStripMenuItem,
            this.событийOnlineToolStripMenuItem,
            this.графикСобытийToolStripMenuItem,
            this.отчетToolStripMenuItem,
            this.архивацияСобытийToolStripMenuItem});
            this.принятыеДанныеToolStripMenuItem.Name = "принятыеДанныеToolStripMenuItem";
            this.принятыеДанныеToolStripMenuItem.Size = new System.Drawing.Size(119, 20);
            this.принятыеДанныеToolStripMenuItem.Text = "Принятые данные";
            // 
            // событияToolStripMenuItem
            // 
            this.событияToolStripMenuItem.Name = "событияToolStripMenuItem";
            this.событияToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.событияToolStripMenuItem.Text = "Архив событий";
            this.событияToolStripMenuItem.Click += new System.EventHandler(this.событияToolStripMenuItem_Click);
            // 
            // событийOnlineToolStripMenuItem
            // 
            this.событийOnlineToolStripMenuItem.Name = "событийOnlineToolStripMenuItem";
            this.событийOnlineToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.событийOnlineToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.событийOnlineToolStripMenuItem.Text = "Событий online";
            this.событийOnlineToolStripMenuItem.Click += new System.EventHandler(this.событийOnlineToolStripMenuItem_Click);
            // 
            // графикСобытийToolStripMenuItem
            // 
            this.графикСобытийToolStripMenuItem.Name = "графикСобытийToolStripMenuItem";
            this.графикСобытийToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.графикСобытийToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.графикСобытийToolStripMenuItem.Text = "График событий";
            this.графикСобытийToolStripMenuItem.Click += new System.EventHandler(this.графикСобытийToolStripMenuItem_Click);
            // 
            // отчетToolStripMenuItem
            // 
            this.отчетToolStripMenuItem.Name = "отчетToolStripMenuItem";
            this.отчетToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.отчетToolStripMenuItem.Text = "Отчет";
            this.отчетToolStripMenuItem.Click += new System.EventHandler(this.отчетToolStripMenuItem_Click);
            // 
            // архивацияСобытийToolStripMenuItem
            // 
            this.архивацияСобытийToolStripMenuItem.Name = "архивацияСобытийToolStripMenuItem";
            this.архивацияСобытийToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.архивацияСобытийToolStripMenuItem.Text = "Архивация событий";
            this.архивацияСобытийToolStripMenuItem.Click += new System.EventHandler(this.архивацияСобытийToolStripMenuItem_Click);
            // 
            // toolsMenu
            // 
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.обновитьСтатусыРайоновToolStripMenuItem,
            this.очиститьКэшToolStripMenuItem,
            this.обновитьВесьКэшToolStripMenuItem,
            this.warnToolStrip,
            this.mapToolStrip,
            this.soundToolStrip,
            this.тестовоеСообщениеToolStripMenuItem,
            this.мониторУзловToolStripMenuItem});
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new System.Drawing.Size(70, 20);
            this.toolsMenu.Text = "Свойства";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.optionsToolStripMenuItem.Text = "Настройки";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // обновитьСтатусыРайоновToolStripMenuItem
            // 
            this.обновитьСтатусыРайоновToolStripMenuItem.Name = "обновитьСтатусыРайоновToolStripMenuItem";
            this.обновитьСтатусыРайоновToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.обновитьСтатусыРайоновToolStripMenuItem.Text = "Обновить статусы районов";
            this.обновитьСтатусыРайоновToolStripMenuItem.Click += new System.EventHandler(this.обновитьСтатусыРайоновToolStripMenuItem_Click);
            // 
            // очиститьКэшToolStripMenuItem
            // 
            this.очиститьКэшToolStripMenuItem.Name = "очиститьКэшToolStripMenuItem";
            this.очиститьКэшToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.очиститьКэшToolStripMenuItem.Text = "Очистить кэш";
            this.очиститьКэшToolStripMenuItem.Click += new System.EventHandler(this.очиститьКэшToolStripMenuItem_Click);
            // 
            // обновитьВесьКэшToolStripMenuItem
            // 
            this.обновитьВесьКэшToolStripMenuItem.Name = "обновитьВесьКэшToolStripMenuItem";
            this.обновитьВесьКэшToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.обновитьВесьКэшToolStripMenuItem.Text = "Обновить весь кэш";
            this.обновитьВесьКэшToolStripMenuItem.Click += new System.EventHandler(this.обновитьВесьКэшToolStripMenuItem_Click);
            // 
            // warnToolStrip
            // 
            this.warnToolStrip.CheckOnClick = true;
            this.warnToolStrip.Name = "warnToolStrip";
            this.warnToolStrip.Size = new System.Drawing.Size(224, 22);
            this.warnToolStrip.Text = "Отключить оповещение";
            // 
            // mapToolStrip
            // 
            this.mapToolStrip.Checked = true;
            this.mapToolStrip.CheckOnClick = true;
            this.mapToolStrip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mapToolStrip.Name = "mapToolStrip";
            this.mapToolStrip.Size = new System.Drawing.Size(224, 22);
            this.mapToolStrip.Text = "Карта";
            this.mapToolStrip.Click += new System.EventHandler(this.картаToolStripMenuItem_Click_1);
            // 
            // soundToolStrip
            // 
            this.soundToolStrip.Checked = true;
            this.soundToolStrip.CheckOnClick = true;
            this.soundToolStrip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.soundToolStrip.Name = "soundToolStrip";
            this.soundToolStrip.Size = new System.Drawing.Size(224, 22);
            this.soundToolStrip.Text = "Звуковое оповещение";
            this.soundToolStrip.Click += new System.EventHandler(this.soundTooItem_Click);
            // 
            // тестовоеСообщениеToolStripMenuItem
            // 
            this.тестовоеСообщениеToolStripMenuItem.Name = "тестовоеСообщениеToolStripMenuItem";
            this.тестовоеСообщениеToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.тестовоеСообщениеToolStripMenuItem.Text = "Тестовое сообщение";
            this.тестовоеСообщениеToolStripMenuItem.Click += new System.EventHandler(this.тестовоеСообщениеToolStripMenuItem_Click);
            // 
            // мониторУзловToolStripMenuItem
            // 
            this.мониторУзловToolStripMenuItem.Name = "мониторУзловToolStripMenuItem";
            this.мониторУзловToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.мониторУзловToolStripMenuItem.Text = "Монитор узлов";
            this.мониторУзловToolStripMenuItem.Click += new System.EventHandler(this.мониторУзловToolStripMenuItem_Click);
            // 
            // окнаToolStripMenuItem
            // 
            this.окнаToolStripMenuItem.Name = "окнаToolStripMenuItem";
            this.окнаToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.окнаToolStripMenuItem.Text = "Окна";
            this.окнаToolStripMenuItem.Visible = false;
            // 
            // входToolStripMenuItem
            // 
            this.входToolStripMenuItem.Name = "входToolStripMenuItem";
            this.входToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.входToolStripMenuItem.Text = "Вход";
            this.входToolStripMenuItem.Click += new System.EventHandler(this.входToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStripButton9});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(632, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::App3.Properties.Resources.kte1386967435034_en_us;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Опции";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::App3.Properties.Resources._455695_200;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Таблица сообщений";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::App3.Properties.Resources.tworows_9707;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "Активность";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::App3.Properties.Resources.w512h5121390849392combo512;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Окно статистики";
            this.toolStripButton4.Visible = false;
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click_1);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::App3.Properties.Resources.Data_List_icon;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "Принятые пакеты";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::App3.Properties.Resources.ThresholdRulenode_8715_24;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "Окно тревожных сообщений";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click_1);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::App3.Properties.Resources.close;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Закрыть";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = global::App3.Properties.Resources.maximize;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton8.Text = "Окно на весь экран";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::App3.Properties.Resources.minimize;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "Минимизировать";
            this.toolStripButton9.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip.Location = new System.Drawing.Point(0, 431);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(632, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(93, 17);
            this.toolStripStatusLabel1.Text = "Синхронизация";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 59);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(200, 0);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(432, 59);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.ConnectCnt);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 59);
            this.panel2.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество подключений";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(161, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "мин.";
            // 
            // ConnectCnt
            // 
            this.ConnectCnt.AutoSize = true;
            this.ConnectCnt.Location = new System.Drawing.Point(182, 10);
            this.ConnectCnt.Name = "ConnectCnt";
            this.ConnectCnt.Size = new System.Drawing.Size(15, 17);
            this.ConnectCnt.TabIndex = 1;
            this.ConnectCnt.Text = "0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(108, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "0";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Время работы";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // синхронизацияОбъектовССервераToolStripMenuItem
            // 
            this.синхронизацияОбъектовССервераToolStripMenuItem.Name = "синхронизацияОбъектовССервераToolStripMenuItem";
            this.синхронизацияОбъектовССервераToolStripMenuItem.Size = new System.Drawing.Size(288, 22);
            this.синхронизацияОбъектовССервераToolStripMenuItem.Text = "Синхронизация объектов с сервера";
            this.синхронизацияОбъектовССервераToolStripMenuItem.Click += new System.EventHandler(this.синхронизацияОбъектовССервераToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ГМК Сполох";
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private ToolStripMenuItem тестовоеСообщениеToolStripMenuItem;
        private ToolStripMenuItem мониторУзловToolStripMenuItem;
        private ToolStripMenuItem отправитьПоследниеСообщенияToolStripMenuItem;
        private ToolStripMenuItem обновитьВесьКэшToolStripMenuItem;
        private ToolStripMenuItem обновитьСтатусыРайоновToolStripMenuItem;
        private ToolStripMenuItem синхронизацияОбъектовССервераToolStripMenuItem;
    }
}



