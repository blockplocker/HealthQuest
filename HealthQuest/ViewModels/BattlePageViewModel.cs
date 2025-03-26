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
            set
            {
                _enemyHP = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEnemyDefeated));
            }
        }

        public int EnemyMaxHP { get; set; }
        public int EnemyStrength { get; set; }
        public int EnemyAgility { get; set; }
        public string EnemyImg { get; set; }

        // Battle Log
        public ObservableCollection<string> BattleLog { get; set; } = new();

        // Commands
        public ICommand AttackCommand { get; }
        public ICommand PowerAttackCommand { get; }
        public ICommand DefendCommand { get; }
        public ICommand SpawnEnemyCommand { get; }

        private Random _random = new();

        public BattlePageViewModel()
        {
            // Initialize player stats from StatsManager
            PlayerHP = StatsManager.Instance.Stats.Hp;
            PlayerStamina = StatsManager.Instance.Stats.Stamina * 10;
            StaminaCost = 10 + (int)(PlayerStamina * 0.1);
            StaminaRegain = 10 + (int)(PlayerStamina * 0.2);

            // Initialize enemy stats
            SpawnNewEnemy();

            // Commands
            AttackCommand = new Command(PlayerAttack, CanAttack);
            PowerAttackCommand = new Command(PlayerPowerAttack, CanPowerAttack);
            DefendCommand = new Command(PlayerDefend);
            SpawnEnemyCommand = new Command(SpawnNewEnemy);
        }

        public bool IsEnemyDefeated => EnemyHP <= 0;

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

        private void SpawnNewEnemy()
        {
            int img = _random.Next(1, 3);
            if(img == 1)
            {
                EnemyImg = "orc_still.gif";
            }
            else if(img == 2)
            {
                EnemyImg = "goblin_still.gif";
            }
            else
            {
            EnemyImg = "golem_still.gif";
            }
            var playerStats = StatsManager.Instance.Stats;
            EnemyMaxHP = _random.Next(playerStats.Hp / 2, playerStats.Vigor * 8);
            EnemyHP = EnemyMaxHP;
            EnemyStrength = _random.Next(playerStats.Strenght / 2, playerStats.Strenght);
            EnemyAgility = _random.Next(playerStats.Agility / 2, playerStats.Agility );
            BattleLog.Add("A new enemy has appeared!");

            OnPropertyChanged(nameof(EnemyImg));
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
