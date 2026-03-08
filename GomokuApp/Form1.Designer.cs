namespace GomokuApp;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private Panel boardPanel = null!;
    private ComboBox difficultyComboBox = null!;
    private ComboBox turnComboBox = null!;
    private Button startButton = null!;
    private Button resetButton = null!;
    private Label statusLabel = null!;
    private Label difficultyValueLabel = null!;
    private Label hintLabel = null!;
    private Label hintDetailLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        boardPanel = new Panel();
        difficultyComboBox = new ComboBox();
        turnComboBox = new ComboBox();
        startButton = new Button();
        resetButton = new Button();
        statusLabel = new Label();
        difficultyValueLabel = new Label();
        hintLabel = new Label();
        hintDetailLabel = new Label();
        var titleLabel = new Label();
        var difficultyLabel = new Label();
        var turnLabel = new Label();

        SuspendLayout();

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 242, 231);
        ClientSize = new Size(980, 640);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "五目並べ";

        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Yu Gothic UI", 22F, FontStyle.Bold);
        titleLabel.Location = new Point(700, 32);
        titleLabel.Text = "五目並べ";

        difficultyLabel.AutoSize = true;
        difficultyLabel.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
        difficultyLabel.Location = new Point(706, 100);
        difficultyLabel.Text = "難易度";

        difficultyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        difficultyComboBox.Font = new Font("Yu Gothic UI", 11F);
        difficultyComboBox.Items.AddRange(["Easy", "Normal", "Hard"]);
        difficultyComboBox.Location = new Point(706, 128);
        difficultyComboBox.Size = new Size(210, 33);

        turnLabel.AutoSize = true;
        turnLabel.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
        turnLabel.Location = new Point(706, 182);
        turnLabel.Text = "先手 / 後手";

        turnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        turnComboBox.Font = new Font("Yu Gothic UI", 11F);
        turnComboBox.Items.AddRange(["先手 (黒)", "後手 (白)"]);
        turnComboBox.Location = new Point(706, 210);
        turnComboBox.Size = new Size(210, 33);

        startButton.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
        startButton.Location = new Point(706, 268);
        startButton.Size = new Size(210, 44);
        startButton.Text = "新しい対局を開始";
        startButton.UseVisualStyleBackColor = true;
        startButton.Click += StartButton_Click;

        resetButton.Font = new Font("Yu Gothic UI", 10F);
        resetButton.Location = new Point(706, 325);
        resetButton.Size = new Size(210, 38);
        resetButton.Text = "盤面をリセット";
        resetButton.UseVisualStyleBackColor = true;
        resetButton.Click += ResetButton_Click;

        statusLabel.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
        statusLabel.Location = new Point(706, 386);
        statusLabel.Size = new Size(230, 52);
        statusLabel.Text = "状態:";

        difficultyValueLabel.Font = new Font("Yu Gothic UI", 10F);
        difficultyValueLabel.Location = new Point(706, 438);
        difficultyValueLabel.Size = new Size(210, 28);
        difficultyValueLabel.Text = "難易度:";

        hintLabel.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
        hintLabel.Location = new Point(706, 480);
        hintLabel.Size = new Size(230, 28);
        hintLabel.Text = "推奨手:";

        hintDetailLabel.Font = new Font("Yu Gothic UI", 9F);
        hintDetailLabel.Location = new Point(706, 512);
        hintDetailLabel.Size = new Size(230, 72);
        hintDetailLabel.Text = "Easy の危険局面でのみ表示されます。";

        boardPanel.BackColor = Color.FromArgb(239, 200, 122);
        boardPanel.BorderStyle = BorderStyle.FixedSingle;
        boardPanel.Location = new Point(24, 24);
        boardPanel.Size = new Size(565, 565);
        boardPanel.Paint += BoardPanel_Paint;
        boardPanel.MouseClick += BoardPanel_MouseClick;

        Controls.Add(titleLabel);
        Controls.Add(difficultyLabel);
        Controls.Add(difficultyComboBox);
        Controls.Add(turnLabel);
        Controls.Add(turnComboBox);
        Controls.Add(startButton);
        Controls.Add(resetButton);
        Controls.Add(statusLabel);
        Controls.Add(difficultyValueLabel);
        Controls.Add(hintLabel);
        Controls.Add(hintDetailLabel);
        Controls.Add(boardPanel);

        ResumeLayout(false);
        PerformLayout();
    }
}
