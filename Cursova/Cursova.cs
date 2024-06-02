using System;
using System.IO;
using System.Data;
using System.Windows.Forms;
namespace Cursova
{
    public partial class Windows_Journal : Form
    {
        public Windows_Journal()
        {
            InitializeComponent();
        }
        // Опис структури
        struct Event
        {
            public string Name;
            public string Lvl;
            public int Code;
            public string Date;
            public string Time;
        }
        // Опис масиву структури
        static Event[] events = new Event[20];
        static int i = 0;
                    //..................................МЕТОДИ ПОШУКУ.....................
        // Метод пошуку подій з рівнем "Помилка"
        static int Lvl_Fail(Event[] events, int n) 
        {
            int find_fale = n;
            for (var i = n; i < events.Length; ++i)
            {
                if (events[i].Lvl == "Помилка")
                {
                    find_fale = i;
                }
            }
            return find_fale;
        }
        // Метод пошуку подій з рівнем "Попередження"
        static int Lvl_Warning(Event[] events, int n) 
        {
            int find_fale = n;
            for (var i = n; i < events.Length; ++i)
            {
                if (events[i].Lvl == "Попередження")
                {
                    find_fale = i;
                }
            }
            return find_fale;
        }
        // Метод пошуку подій за сьогодні
        static int Today(Event[] events, string dt1, int n)
        {
            var currentDateTime = DateTime.Now;
            var dt2 = currentDateTime.ToString("dd.MM.yyyy");
            dt1 = dt2;
            int find_date1 = n;
            for (var i = n; i < events.Length; ++i)
            {
                if (events[i].Date == dt1)
                {
                    find_date1 = i;
                }
            }
            return find_date1;
        }
        // Метод пошуку подій за датою
        static int Date(Event[] events, string dt, int n) 
        {
            int find_date = n;
            for (var i = n; i < events.Length; ++i)
            {
                if (events[i].Date == dt)
                {
                    find_date = i;
                }
            }
            return find_date;
        }
        //Метод для обміну елементів
        static void Swap(ref Event x, ref Event y)
        {
            var t = x;
            x = y;
            y = t;
        }
                        //...............................МЕТОДИ СОРТУВАННЯ ПОДІЙ ВИБОРОМ............................................................................
        // Метод сортування подій за помилкою вибором
        static void SelectionSort_forFails(ref Event[] events, int currentIndex = 0)
        {
            if (currentIndex == events.Length)
                return;
            var index = Lvl_Fail(events, currentIndex);
            if (index != currentIndex)
            {
                Swap(ref events[index], ref events[currentIndex]);
            }
             SelectionSort_forFails(ref events, currentIndex + 1);
        }
        // Метод сортування подій за датою вибором
        static void SelectionSort_forDate(ref Event[] events, string date, int currentIndex = 0)
        {
            if (currentIndex == events.Length)
                return;
            var index = Date(events, date, currentIndex);
            if (index != currentIndex)
            {
                Swap(ref events[index], ref events[currentIndex]);
            }
            SelectionSort_forDate(ref events, date, currentIndex + 1);
        }
        // Метод сортування подій за попередженням вибором
        static void SelectionSort_forWarnings(ref Event[] events, int currentIndex = 0)
        {
            if (currentIndex == events.Length)
                return;
            var index = Lvl_Warning(events, currentIndex);
            if (index != currentIndex)
            {
                Swap(ref events[index], ref events[currentIndex]);
            }
            SelectionSort_forWarnings(ref events, currentIndex + 1);
        }
        // Метод сортування подій за сьогодні вибором
        static void SelectionSort_forToday(ref Event[] events, string date, int currentIndex = 0)
        {
            if (currentIndex == events.Length)
                return;
            var index = Today(events, date, currentIndex);
            if (index != currentIndex)
            {
                Swap(ref events[index], ref events[currentIndex]);
            }
            SelectionSort_forToday(ref events, date, currentIndex + 1);
        }
                    //..........................МЕТОДИ ПОСЛІДОВНОГО ПОШУКУ...................................................
        // Послідовний пошук попереджень за сьогодні
        static Event[] Sequential_Search_forWarnings_Today(Event[] events, string dt)
        {
            // Виклик методу сортування подій за рівнем "Попередження"
            SelectionSort_forWarnings(ref events);
            // Виклик методу сортування подій за сьогодні
            SelectionSort_forToday(ref events, dt);
            int occurences = 0;
            Event[] foundEvents = new Event[events.Length];
            foreach (Event element in events)
            {
                if (element.Lvl == "Попередження" && element.Date == dt)
                {
                    foundEvents[occurences] = element;
                    occurences++;
                }
            }
            Array.Resize(ref foundEvents, occurences);
            return foundEvents;
        }
        // Послідовний пошук помилок за датою
        static Event [] Sequential_Search_forFails_Date(Event[] events, string dt)
        {
            // Виклик методу сортування подій за рівнем "Помилка"
            SelectionSort_forFails(ref events);
            // Виклик методу сортування подій за датою
            SelectionSort_forDate(ref events, dt);
            int occurences = 0;
            Event[] foundEvents = new Event[events.Length];
            foreach (Event element in events)
            {
                if (element.Lvl == "Помилка" && element.Date == dt)
                {
                    foundEvents[occurences] = element;
                    occurences++;
                }
            }
            Array.Resize(ref foundEvents, occurences);
            return foundEvents;
        }
        // Послідовний пошук подій за датою
        static Event [] Sequential_Search_forDate(Event[] events, string dt)
        {
            int occurences1 = 0;
            // Виклик методу сортування подій за датою
            SelectionSort_forDate(ref events, dt);
            Event[] findEvents = new Event[events.Length];
            foreach (Event element1 in events)
            {
                if (element1.Date == dt)
                {
                    findEvents[occurences1] = element1;
                    occurences1++;
                }
            }
            Array.Resize (ref findEvents, occurences1);
            return findEvents;
        }
        // Послідовний пошук помилок за останній місяць
        static Event[] SequentialSearch_forFailsLastMonth(Event[] events, string lastmonth)
        {
            Event[] foundEvents = new Event[0];
            foreach (Event evt in events)
            {
                if (evt.Date == lastmonth || evt.Lvl == "Помилка")
                {
                    Array.Resize(ref foundEvents, foundEvents.Length + 1);
                    foundEvents[foundEvents.Length - 1] = evt;
                }
            }
            return foundEvents;
        }
                        //..................................РЕАЛІЗАЦІЯ КНОПОК ПРОГРАМИ..............................................
        // Кнопка для додавання події у таблицю
        private void AddEvent_Click(object sender, EventArgs e)
        {
            // Перевірка, чи всі дані введені
            if (txtCode.Text == "" || txtName.Text == "" || txtTime.Text == "" || cmbLvl.SelectedIndex == -1 || dtpDate.Text == "")
            {
                MessageBox.Show("Не всі дані обрано!", "Помилка.");
            }
            else
            {
                // Перевірка правильності формату часу
                if (TimeSpan.TryParseExact(txtTime.Text, "hh\\:mm", null, out _))
                {
                    // Перевірка правильності формату коду
                    if (txtCode.Text.Length == 4 && int.TryParse(txtCode.Text, out _))
                    {
                        // Перевірка на унікальність події
                        bool isEventUnique = true;
                        foreach (DataGridViewRow row in dgvLib.Rows)
                        {
                            if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == txtName.Text &&
                                row.Cells[1].Value != null && row.Cells[1].Value.ToString() == cmbLvl.Text &&
                                row.Cells[2].Value != null && row.Cells[2].Value.ToString() == txtCode.Text &&
                                row.Cells[3].Value != null && row.Cells[3].Value.ToString() == dtpDate.Value.ToString("dd.MM.yyyy") &&
                                row.Cells[4].Value != null && row.Cells[4].Value.ToString() == txtTime.Text)
                            {
                                isEventUnique = false;
                                break;
                            }
                        }
                        if (!isEventUnique)
                        {
                            MessageBox.Show("Така подія вже існує!", "Помилка!");
                        }
                        else
                        {
                            events[i].Name = txtName.Text;
                            events[i].Lvl = cmbLvl.Text;
                            events[i].Code = Convert.ToInt32(txtCode.Text);
                            events[i].Date = dtpDate.Value.ToString("dd.MM.yyyy");
                            events[i].Time = txtTime.Text;
                            dgvLib.Rows.Add(events[i].Name, events[i].Lvl, events[i].Code.ToString(), events[i].Date, events[i].Time);
                            i++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Код повинен містити 4 цифри.", "Помилка!");
                    }
                }
                else
                {
                    MessageBox.Show("Неправильний формат часу! Введіть час у форматі 'HH:MM'", "Помилка.");
                }
            }
        }
        // Кнопка видалення події з таблиці
        private void DelEvent_Click(object sender, EventArgs e)
        {
            if (dgvLib.SelectedRows.Count > 0)
            {
                if (dgvLib.SelectedRows.Count > 0)
                {
                    // Обробка такого винятку, коли користувач спробує видалити порожній рядок
                    if (dgvLib.SelectedRows[0].Cells[0].Value != null)
                    {
                        dgvLib.Rows.RemoveAt(dgvLib.SelectedRows[0].Index);
                    }
                    else
                    {
                        MessageBox.Show("Не можна видалити порожній рядок.", "Помилка!");
                    }
                }
                else
                {
                    MessageBox.Show("Оберіть рядок для видалення.", "Помилка!");
                }
            }
        }
        // Кнопка очищення бібліотеки
        private void ClearLib_Click(object sender, EventArgs e)
        {
            if (dgvLib.Rows.Count > 0)
            {
                dgvLib.Rows.Clear();
            }
            else
            {
                MessageBox.Show("Таблиця пуста.", "Помилка");
            }
        }
        // Ця подія реалізована для того, щоб при натисканні на будь-який стовпець в dgvLib не з'являлась помилка
        private void dgvLib_MouseClick(object sender, MouseEventArgs e)
        {
            //txtName.Text = dgvLib.SelectedRows[0].Cells[0].Value.ToString();
            //cmbLvl.Text = dgvLib.SelectedRows[0].Cells[1].Value.ToString();
            //txtCode.Text = dgvLib.SelectedRows[0].Cells[2].Value.ToString();
            //dtpDate.Text = dgvLib.SelectedRows[0].Cells[3].Value.ToString();
            //dtpTime.Text = dgvLib.SelectedRows[0].Cells[4].Value.ToString();
        }
        // Кнопка для збереження бібліотеки у файл XML
        private void Save_as_XML_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.TableName = "Event";
                dt.Columns.Add("Name");
                dt.Columns.Add("Lvl");
                dt.Columns.Add("Code");
                dt.Columns.Add("Date");
                dt.Columns.Add("Time");
                ds.Tables.Add(dt);
                foreach (DataGridViewRow r in dgvLib.Rows)
                {
                    DataRow row = ds.Tables["Event"].NewRow();
                    row["Name"] = r.Cells[0].Value;
                    row["Lvl"] = r.Cells[1].Value;
                    row["Code"] = r.Cells[2].Value;
                    row["Date"] = r.Cells[3].Value;
                    row["Time"] = r.Cells[4].Value;
                    ds.Tables["Event"].Rows.Add(row);
                }
                ds.WriteXml("Windows_Journal.xml");
                MessageBox.Show("XML-файл успішно збережено!", "Виконано.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні XML-файлу: {ex.Message}", "Помилка.");
            }
        }
        // Кнопка завантаження XML файлу
        private void DnlXML_Click(object sender, EventArgs e)
        {
            dgvLib.Rows.Clear();
            if (File.Exists("Windows_Journal.xml"))
            {
                DataSet ds = new DataSet();
                ds.ReadXml("Windows_Journal.xml");
                foreach (DataRow item in ds.Tables["Event"].Rows)
                {
                    int n = dgvLib.Rows.Add();
                    dgvLib.Rows[n].Cells[0].Value = item["Name"];
                    dgvLib.Rows[n].Cells[1].Value = item["Lvl"];
                    dgvLib.Rows[n].Cells[2].Value = item["Code"];
                    dgvLib.Rows[n].Cells[3].Value = item["Date"];
                    dgvLib.Rows[n].Cells[4].Value = item["Time"];
                }
            }
            else
            {
                MessageBox.Show("XML-файл не знайдено!", "Помилка.");
            }
        }
        // Кнопка пошуку помилок за датою
        private void btnErrwithDt_Click(object sender, EventArgs e)
        {
            // Очищення dgvErrwithDt перед додаванням нових даних
            // Без очищення dgvErrwithDt нові дані будуть неправильно відображені: нові дані(за критеріями). Виникне помилка
            // додадуться до існуючих даних(з dgvLib), що вийде сумішю нових і старих даних
            dgvErrwithDt.Rows.Clear();
            string enteredDate = dtpDate.Value.ToString("dd.MM.yyyy");
            Event[] findEvents = Sequential_Search_forFails_Date(events, enteredDate);
            foreach (Event evt in findEvents)
            {
                dgvErrwithDt.Rows.Add(evt.Name, evt.Lvl, evt.Code.ToString(), evt.Date, evt.Time);
            }
            if (findEvents.Length == 0)
            {
                MessageBox.Show("Помилок за вказаною датою не знайдено.", "Інформація.");
            }
        }
        // Кнопка для редагування події у бібліотеці
        private void ChangeEvent_Click(object sender, EventArgs e)
        {
            if (dgvLib.SelectedRows.Count > 0)
            {
                int n = dgvLib.SelectedRows[0].Index;
                dgvLib.Rows[n].Cells[0].Value = txtName.Text;
                dgvLib.Rows[n].Cells[1].Value = cmbLvl.Text;
                dgvLib.Rows[n].Cells[2].Value = txtCode.Text;
                dgvLib.Rows[n].Cells[3].Value = dtpDate.Text;
                dgvLib.Rows[n].Cells[4].Value = txtTime.Text;
            }
            else
            {
                MessageBox.Show("Оберіть строку для редагування.", "Помилка.");
            }
        }
        // Кнопка пошуку подій за датою
        private void btnDT_Click(object sender, EventArgs e)
        {
            dgvDate.Rows.Clear();
            string enteredDate = dtpTime.Value.ToString("dd.MM.yyyy");
            Event[] findevents = Sequential_Search_forDate(events, enteredDate);
            foreach (Event evt in findevents)
            {
                dgvDate.Rows.Add(evt.Name, evt.Lvl, evt.Code.ToString(), evt.Date, evt.Time);
            }
            if (findevents.Length == 0)
            {
                MessageBox.Show("Подій за вказаною датою не знайдено.", "Інформація.");
            }
        }
        // Кнопка пошуку попереджень за сьогодні
        private void btn_Wrn_Today_Click(object sender, EventArgs e)
        {
            dgvWrn_TD.Rows.Clear();
            string enteredDate = dtpToday.Value.ToString("dd.MM.yyyy");
            Event[] findEvents = Sequential_Search_forWarnings_Today(events, enteredDate);
            foreach (Event evt in findEvents)
            {
                dgvWrn_TD.Rows.Add(evt.Name, evt.Lvl, evt.Code.ToString(), evt.Date, evt.Time);
            }
            if (findEvents.Length == 0)
            {
                MessageBox.Show("Попереджень за сьогодні не знайдено.", "Інформація.");
            }
            int wrncount = findEvents.Length;
            txtWrn.Text = wrncount.ToString();
        }
        // Кнопка отримання кількості днів з моменту події
        private void btnCount_Click(object sender, EventArgs e)
        {
            dgvCount.Rows.Clear();
            // Використовуємо Rows.Count - 1, щоб уникнути обробки останнього пустого рядка
            for (int i = 0; i < dgvLib.Rows.Count - 1; i++) 
            {
                string[] parts = dgvLib.Rows[i].Cells[3].Value.ToString().Split('.');
                if (parts.Length == 3 && int.TryParse(parts[2], out int year) && int.TryParse(parts[1], out int month) && int.TryParse(parts[0], out int day))
                {
                    DateTime tmpdate = new DateTime(year, month, day);
                    TimeSpan dif = DateTime.Today - tmpdate;
                    int count = dif.Days;
                    dgvCount.Rows.Add();
                    dgvCount.Rows[i].Cells[0].Value = dgvLib.Rows[i].Cells[0].Value;
                    dgvCount.Rows[i].Cells[1].Value = dgvLib.Rows[i].Cells[1].Value;
                    dgvCount.Rows[i].Cells[2].Value = dgvLib.Rows[i].Cells[2].Value;
                    dgvCount.Rows[i].Cells[3].Value = dgvLib.Rows[i].Cells[3].Value;
                    dgvCount.Rows[i].Cells[4].Value = dgvLib.Rows[i].Cells[4].Value;
                    dgvCount.Rows[i].Cells[5].Value = count; // додати кількість днів як нову колонку
                }
            }
        }
        //Кнопка пошуку помилок за останній місяць
        private void Fails_Last_M_Click(object sender, EventArgs e)
        {
            dgvLastMonthFails.Rows.Clear();
            var Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var LastMonth = Month.AddMonths(-1);
            Event[] lastMonthEvents = SequentialSearch_forFailsLastMonth(events, LastMonth.ToString("dd.MM.yyyy"));
            if (lastMonthEvents.Length == 0)
            {
                MessageBox.Show("Подій за останній місяць не знайдено.", "Інформація");
                return;
            }
            foreach (Event evt in lastMonthEvents)
            {
                if (evt.Lvl == "Помилка")
                {
                    dgvLastMonthFails.Rows.Add(evt.Name, evt.Lvl, evt.Code.ToString(), evt.Date, evt.Time);
                }
            }
        }
    }
}
