using HealthQuest.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HealthQuest.ViewModels
{
    public class BattlePageViewModel : INotifyPropertyChanged
    {
        // Player properties
        private int _playerHP;
        public int PlayerHP
        {
            get => _playerHP;
            set
            {
                _playerHP = value;
                OnPropertyChanged();
                SavePlayerStatsAsync();
            }
        }
        private int _playerStamina;
        public int PlayerStamina
        {
            get => _playerStamina;
            set
            {
                _playerStamina = value;
                OnPropertyChanged();
            }
        }
        public int StaminaCost { get; set; }
        public int StaminaRegain { get; set; }

        // Enemy properties
        private int _enemyHP;
        public int EnemyHP
        {
            get => _enemyHP;
            set { _enemyHP = value; OnPropertyChanged(); }
        }

        public int EnemyMaxHP { get; set; }
        public int EnemyStrength { get; set; }
        public int EnemyAgility { get; set; }

        // Battle Log
        public ObservableCollection<string> BattleLog { get; set; } = new();

        // Commands
        public ICommand AttackCommand { get; }
        public ICommand PowerAttackCommand { get; }
        public ICommand DefendCommand { get; }

        private Random _random = new();

        public BattlePageViewModel()
        {
            // Initialize player stats from StatsManager
            PlayerHP = StatsManager.Instance.Stats.Hp;
            PlayerStamina = StatsManager.Instance.Stats.Stamina * 10;
            StaminaCost = 10 + (int)(PlayerStamina * 0.1);
            StaminaRegain = 10 + (int)(PlayerStamina * 0.2);

            // Initialize enemy stats
            EnemyMaxHP = 80;
            EnemyHP = EnemyMaxHP;
            EnemyStrength = 12;
            EnemyAgility = 8;

            // Commands
            AttackCommand = new Command(PlayerAttack, CanAttack);
            PowerAttackCommand = new Command(PlayerPowerAttack, CanPowerAttack);
            DefendCommand = new Command(PlayerDefend);
        }

        private bool CanAttack() => PlayerHP > 0 && EnemyHP > 0 && PlayerStamina >= StaminaCost;
        private bool CanPowerAttack() => PlayerHP > 0 && EnemyHP > 0 && PlayerStamina >= StaminaCost * 3;

        private void PlayerAttack()
        {
            PlayerStamina -= StaminaCost;

            int damage = CalculateDamage(StatsManager.Instance.Stats.Strenght, EnemyAgility);
            EnemyHP -= damage;
            BattleLog.Add($"You attack and deal {damage} damage! (Stamina cost: {StaminaCost})");

            if (EnemyHP <= 0)
            {
                BattleLog.Add("You have defeated the enemy!");
                return;
            }

            EnemyTurn();
        }

        private void PlayerPowerAttack()
        {
            PlayerStamina -= StaminaCost * 3;

            int damage = CalculateDamage(StatsManager.Instance.Stats.Strenght, EnemyAgility) * 2;
            EnemyHP -= damage;
            BattleLog.Add($"You perform a power attack and deal {damage} damage! (Stamina cost: {StaminaCost * 3})");

            if (EnemyHP <= 0)
            {
                BattleLog.Add("You have defeated the enemy!");
                return;
            }
            EnemyTurn();
        }

        private void PlayerDefend()
        {
            PlayerStamina += StaminaRegain;

            BattleLog.Add($"You brace for impact, reducing incoming damage and regain {StaminaRegain} stamina.");
            EnemyTurn(true);
        }

        private void EnemyTurn(bool playerDefending = false)
        {
            int damage = CalculateDamage(EnemyStrength, StatsManager.Instance.Stats.Agility, playerDefending);
            if (playerDefending)
            {
                damage /= 2; // Reduce damage if defending
            }
            PlayerHP -= damage;
            BattleLog.Add($"Enemy attacks and deals {damage} damage!");

            if (PlayerHP <= 0)
            {
                BattleLog.Add("You have been defeated!");
            }
        }

        private int CalculateDamage(int attackerStrength, int defenderAgility, bool playerDefending = false)
        {
            int baseDamage = attackerStrength + _random.Next(-5, 6); // Small random variation
            double dodgeChance = CalculateDodgeChance(defenderAgility, playerDefending);
            int dodgeRoll = _random.Next(1, 101);
            if (dodgeRoll <= dodgeChance)
            {
                BattleLog.Add("Attack missed!");
                return 0;
            }
            return Math.Max(baseDamage, 1); // Ensure at least 1 damage
        }

        private double CalculateDodgeChance(int agility, bool playerDefending)
        {
            // Calculate dodge chance with a curve, max 30%
            double maxDodgeChance = 30.0;
            double dodgeChance = maxDodgeChance * (1 - Math.Exp(-0.02 * agility));
            if (playerDefending)
            {
                dodgeChance *= 2; // Double the dodge chance if defending
            }
            return dodgeChance;
        }

        private async void SavePlayerStatsAsync()
        {
            StatsManager.Instance.Stats.Hp = PlayerHP;
            await StatsManager.Instance.SaveStatsAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
