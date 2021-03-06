﻿namespace NerdBlock.Engine.Frontend.Winforms.Views
{
    partial class ViewEditOrder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvItems = new NerdBlock.Engine.Frontend.Winforms.Implementation.DataGridDisplayView();
            this.label3 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.lblOrderedBy = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblOrderCost = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.clmProdId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmProdName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 42;
            this.label1.Text = "View Order";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvItems);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 260);
            this.groupBox1.TabIndex = 82;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Order Items";
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmProdId,
            this.clmProdName,
            this.clmCost,
            this.clmQuantity});
            this.dgvItems.Location = new System.Drawing.Point(28, 20);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(408, 234);
            this.dgvItems.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 84;
            this.label3.Text = "Order Date";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(255, 61);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(45, 13);
            this.label34.TabIndex = 85;
            this.label34.Text = "Supplier";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 86;
            this.label5.Text = "Ordered By";
            // 
            // lblOrderDate
            // 
            this.lblOrderDate.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblOrderDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrderDate.Location = new System.Drawing.Point(86, 61);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(136, 14);
            this.lblOrderDate.TabIndex = 88;
            // 
            // lblOrderedBy
            // 
            this.lblOrderedBy.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblOrderedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrderedBy.Location = new System.Drawing.Point(86, 85);
            this.lblOrderedBy.Name = "lblOrderedBy";
            this.lblOrderedBy.Size = new System.Drawing.Size(136, 14);
            this.lblOrderedBy.TabIndex = 89;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(243, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 90;
            this.label6.Text = "Order Cost";
            // 
            // lblSupplier
            // 
            this.lblSupplier.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblSupplier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSupplier.Location = new System.Drawing.Point(306, 62);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(136, 14);
            this.lblSupplier.TabIndex = 91;
            // 
            // lblOrderCost
            // 
            this.lblOrderCost.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblOrderCost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrderCost.Location = new System.Drawing.Point(306, 85);
            this.lblOrderCost.Name = "lblOrderCost";
            this.lblOrderCost.Size = new System.Drawing.Size(136, 14);
            this.lblOrderCost.TabIndex = 92;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "Order ID";
            // 
            // lblOrderId
            // 
            this.lblOrderId.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblOrderId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOrderId.Location = new System.Drawing.Point(86, 36);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(45, 16);
            this.lblOrderId.TabIndex = 87;
            // 
            // clmProdId
            // 
            this.clmProdId.DataPropertyName = "ProductId";
            this.clmProdId.HeaderText = "Product ID";
            this.clmProdId.Name = "clmProdId";
            this.clmProdId.ReadOnly = true;
            // 
            // clmProdName
            // 
            this.clmProdName.DataPropertyName = "ProductName";
            this.clmProdName.HeaderText = "Product Name";
            this.clmProdName.Name = "clmProdName";
            this.clmProdName.ReadOnly = true;
            // 
            // clmCost
            // 
            this.clmCost.DataPropertyName = "Cost";
            this.clmCost.HeaderText = "Batch Cost";
            this.clmCost.Name = "clmCost";
            this.clmCost.ReadOnly = true;
            // 
            // clmQuantity
            // 
            this.clmQuantity.DataPropertyName = "Quantity";
            this.clmQuantity.HeaderText = "Quantity";
            this.clmQuantity.Name = "clmQuantity";
            this.clmQuantity.ReadOnly = true;
            // 
            // ViewEditOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblOrderCost);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblOrderedBy);
            this.Controls.Add(this.lblOrderDate);
            this.Controls.Add(this.lblOrderId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "ViewEditOrder";
            this.Size = new System.Drawing.Size(491, 394);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.Label lblOrderedBy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblOrderCost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOrderId;
        private Implementation.DataGridDisplayView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmProdId;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmProdName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmQuantity;
    }
}
