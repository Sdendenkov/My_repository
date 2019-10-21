using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BO_GRBS.Entities;
using BO_GRBS.DataController;

namespace BO_GRBS.UI.BS_Project
{
    public partial class Cash_Plan_Template_Redactor : Form
    {
        public Cash_Plan_Template_Redactor()
        {
            InitializeComponent();
        }
        string CODE_DEP = "01";
        public Cash_Plan_Template_Redactor(string codeDep)
        {
            CODE_DEP = codeDep;
            InitializeComponent();
        }

        List<Cash_Plan_Department_Template> departmentTemplate;
        private void Cash_Plan_Template_Redactor_Load(object sender, EventArgs e)
        {
            /// <summary>
            /// Код финансового департамента.(GlobalS.Settings.CurrentUser.DepartmentGRBS.Code);
            /// </summary>
            // шаблон показателей бюджетной сметы
            List<Cash_Plan_Department> listTemplate = new List<Cash_Plan_Department>();
            // шаблон показателей бюджетной сметы
            departmentTemplate = new List<Cash_Plan_Department_Template>();
            string errMess = "";
            listTemplate.Clear();
            departmentTemplate.Clear();
            departmentTemplate = Cash_Plan_Department_Template.GetByCodeDep(CODE_DEP, out errMess);

            updateDataSource(departmentTemplate);
        }

        /// <summary>
        /// Обновление источника данных гриды
        /// </summary>
        /// <param name="source">Сам источник</param>
        private void updateDataSource(List<Cash_Plan_Department_Template> source)
        {
            gridData.DataSource = source;
            gridData.RefreshDataSource();
        }

        private void добавитьСтрокуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRow(false);
            richTextBoxInfo.Text += "Вы добавили строку";
            richTextBoxInfo.Text += "\r\n";
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            addRow(false);
            richTextBoxInfo.Text += "Вы добавили строку";
            richTextBoxInfo.Text += "\r\n";
        }


        private void checkBoxAction_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAction.Checked)
            {
                buttonAction.Text = "Добавить строку";
            }
            else
            {
                buttonAction.Text = "Удалить строку";
            }
        }

        /// <summary>
        /// Метод добавления ЦА И ТОГС строк
        /// </summary>
        /// <param name="isCAandTOGS"></param>
        private void addRow(bool isCAandTOGS)
        {
            if (isCAandTOGS)
            {
                int index = viewData.GetFocusedDataSourceRowIndex();
                Cash_Plan_Department_Template focusedDep = departmentTemplate[index];
                if (focusedDep.IsTotal == false)
                {
                    Cash_Plan_Department_Template totalDep = new Cash_Plan_Department_Template();
                    totalDep.ID = Guid.NewGuid();
                    totalDep.DateReport = DateTime.Now;
                    totalDep.IsTotal = false;
                    totalDep.IsDetail = true;
                    totalDep.IsCA = true;
                    totalDep.NameTypeExp = "ЦА:";
                    totalDep.CodeTargetItem = focusedDep.CodeTargetItem;
                    totalDep.CodeDep = focusedDep.CodeDep;
                    totalDep.CodeCategory = focusedDep.CodeCategory;
                    totalDep.DocState = 0;
                    totalDep.CodeCriterion = focusedDep.CodeCriterion;
                    totalDep.CodeParent = focusedDep.CodeCriterion;
                    totalDep.CodeTypeExp = focusedDep.CodeTypeExp;
                    totalDep.CodeKosgu = focusedDep.CodeKosgu;



                    Cash_Plan_Department_Template TOGSRow = new Cash_Plan_Department_Template();
                    TOGSRow.ID = Guid.NewGuid();
                    TOGSRow.DateReport = DateTime.Now;
                    TOGSRow.IsTotal = false;
                    TOGSRow.IsDetail = true;
                    TOGSRow.IsTOGS = true;
                    TOGSRow.IsCA = false;
                    TOGSRow.NameTypeExp = "ТОГС" + ":";
                    TOGSRow.CodeTargetItem = focusedDep.CodeTargetItem;
                    TOGSRow.CodeDep = focusedDep.CodeDep;
                    TOGSRow.CodeCategory = focusedDep.CodeCategory;
                    TOGSRow.DocState = 0;
                    TOGSRow.CodeCriterion = focusedDep.CodeCriterion;
                    TOGSRow.CodeParent = focusedDep.CodeCriterion;
                    TOGSRow.CodeTypeExp = focusedDep.CodeTypeExp;
                    TOGSRow.CodeKosgu = focusedDep.CodeKosgu;


                    focusedDep.NameTypeExp = focusedDep.CodeTypeExp + "," + " " + "в том числе" + ":";
                    focusedDep.IsTotal = true;

                    departmentTemplate.Insert(index + 1, totalDep);
                    departmentTemplate.Insert(index + 2, TOGSRow);
                    modifyOrder(departmentTemplate);
                    gridData.DataSource = departmentTemplate;
                    gridData.RefreshDataSource();
                }
                else
                {
                    MessageBox.Show("Нельзя добавить к итоговой строке");
                }
            }
            else
            {
                int index = viewData.GetFocusedDataSourceRowIndex();
                Cash_Plan_Department_Template focusedDep = departmentTemplate[index];
                Cash_Plan_Department_Template totalDep = new Cash_Plan_Department_Template();
                totalDep.ID = Guid.NewGuid();
                totalDep.DateReport = DateTime.Now;
                // totalDep.IsTotal = true;
                // totalDep.IsDetail = false;
                totalDep.CodeTargetItem = focusedDep.CodeTargetItem;
                totalDep.CodeDep = focusedDep.CodeDep;
                totalDep.CodeCategory = focusedDep.CodeCategory;
                totalDep.DocState = 0;
                totalDep.CodeParent = null;
                totalDep.NameTypeExp = totalDep.CodeTypeExp = focusedDep.CodeTypeExp;
                totalDep.CodeKosgu = focusedDep.CodeKosgu;

                departmentTemplate.Insert(index, totalDep);
                modifyOrder(departmentTemplate);
                gridData.DataSource = departmentTemplate;
                gridData.RefreshDataSource();
            }

        }

        private void cmnuAddTotal_Click(object sender, EventArgs e)
        {
            
            // focusedDep.
        }
        private void modifyOrder(List<Cash_Plan_Department_Template> departmentTemplate)
        {
            int numOrder = 1000000;
            foreach (Cash_Plan_Department_Template dep in departmentTemplate)
            {
                dep.NumOrder = numOrder++;
            }
        }

        private void добавитьСтрокуДляЦАToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int index = viewData.GetFocusedDataSourceRowIndex();
            //Cash_Plan_Department_Template focusedDep = departmentTemplate[index];
            //Cash_Plan_Department_Template rowCA = new Cash_Plan_Department_Template();

            //focusedDep.CodeTypeExp += ", в том числе:";
            //focusedDep.IsTotal = true;

            //rowCA.ID = Guid.NewGuid();
            //rowCA.DateReport = selectedDateReport;
            //rowCA.IsTotal = false;
            //rowCA.IsDetail = true;
            //rowCA.IsCA = true;
            //rowCA.CodeTargetItem = focusedDep.CodeTargetItem;
            //rowCA.CodeDep = focusedDep.CodeDep;
            //rowCA.CodeCategory = focusedDep.CodeCategory;
            //rowCA.DocState = 0;
            //rowCA.CodeParent = focusedDep.CodeCriterion;
            //rowCA.CodeTypeExp = "ЦА:";
            //rowCA.CodeKosgu = focusedDep.CodeKosgu;
            ////Сумируем код показателя для итоговой строки

            //Cash_Plan_Department_Template rowTOGS = new Cash_Plan_Department_Template();

            //rowTOGS.ID = Guid.NewGuid();
            //rowTOGS.DateReport = selectedDateReport;
            //rowTOGS.IsTotal = false;
            //rowTOGS.IsDetail = true;
            //rowTOGS.IsTOGS = true;
            //rowTOGS.CodeTargetItem = focusedDep.CodeTargetItem;
            //rowTOGS.CodeDep = focusedDep.CodeDep;
            //rowTOGS.CodeCategory = focusedDep.CodeCategory;
            //rowTOGS.DocState = 0;
            //rowTOGS.CodeParent = focusedDep.CodeCriterion;
            //rowTOGS.CodeTypeExp = "ТОГС:";
            //rowTOGS.CodeKosgu = focusedDep.CodeKosgu;

            ////***************************************************************
            //departmentTemplate.Insert(index + 1, rowCA);
            //departmentTemplate.Insert(index + 2, rowTOGS);
            //modifyOrder();
            //gridData.DataSource = departmentTemplate;
            //gridData.RefreshDataSource();
            // focusedDep.
        }

        private void saveTemplate()
        {
            if (MessageBox.Show("Сохранить изменения?", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string errMess = "";
                bool failed = false;
                foreach (Cash_Plan_Department_Template dep in departmentTemplate)
                {
                    if (Cash_Plan_Department_Template.SaveOrUpdate(dep, out errMess) != SetState.Succed)
                    {
                        failed = true;
                        MessageBox.Show(errMess, "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (!failed && departmentTemplate.Count > 0)
                    MessageBox.Show("Сохранено", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveTemplate();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            gridData.DataSource = departmentTemplate;
            gridData.RefreshDataSource();
        }

        /// <summary>
        /// Подъем строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRowUp_Click(object sender, EventArgs e)
        {
            int index = viewData.GetFocusedDataSourceRowIndex();
            if (index != 0)
            {
                Cash_Plan_Department_Template focusedRow = departmentTemplate.OrderBy(x => x.NumOrder).ToList()[index];
                Cash_Plan_Department_Template upperRow = departmentTemplate.OrderBy(x => x.NumOrder).ToList()[index - 1];
                int focusedOrder = focusedRow.NumOrder;
                int upperOrder = upperRow.NumOrder;
                upperRow.NumOrder = focusedOrder;
                focusedRow.NumOrder = upperOrder;

                gridData.DataSource = departmentTemplate.OrderBy(x => x.NumOrder).ToList();

                gridData.RefreshDataSource();
                viewData.FocusedRowHandle = index - 1;
            }
        }
        /// <summary>
        /// Опуск строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRowDown_Click(object sender, EventArgs e)
        {
            int index = viewData.GetFocusedDataSourceRowIndex();
            if (index + 1 < departmentTemplate.Count)
            {
                Cash_Plan_Department_Template focusedRow = departmentTemplate.OrderBy(x => x.NumOrder).ToList()[index];
                Cash_Plan_Department_Template lowerRow = departmentTemplate.OrderBy(x => x.NumOrder).ToList()[index + 1];
                int focusedOrder = focusedRow.NumOrder;
                int upperOrder = lowerRow.NumOrder;
                lowerRow.NumOrder = focusedOrder;
                focusedRow.NumOrder = upperOrder;

                gridData.DataSource = departmentTemplate.OrderBy(x => x.NumOrder).ToList();

                gridData.RefreshDataSource();
                viewData.FocusedRowHandle = index + 1;
            }
        }

        private void цАToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRow(true);
        }

    }
}
