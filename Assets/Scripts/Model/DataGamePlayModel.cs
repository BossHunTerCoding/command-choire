using System;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    public class DataGamePlay
    {
        [field: SerializeField] public int HP { get; private set; } = DataGlobal.HpDefault;
        [field: SerializeField] public int Mail { get; private set; } = DataGlobal.MailDefault;
        [field: SerializeField] public float time = 0f;
        [field: SerializeField] public int countReply = 0;
        [field: SerializeField] public int commandCountTime { get; private set; } = 0;
        [field: SerializeField] public int PercentScore { get; private set; } = DataGlobal.ScoreDefault;
        [field: SerializeField] public int commandUsedCount { get; private set; } = 0;
        [field: SerializeField] public List<MailComponent> MailObjects { get; private set; } = new();
        [field: SerializeField] public List<DogComponent> EnemyObjects { get; private set; } = new();
        [field: SerializeField] public List<TroughComponent> TroughObjects { get; private set; } = new();
        public bool waitCoolDownJump = false;
        public bool playActionCommand = false;
        public bool activeSelectSkipToMode = false;
        public bool OnDragCommand = false;

        public bool CanClickButton => !playActionCommand && !activeSelectSkipToMode && !OnDragCommand;
        public bool CanDargCommand => !playActionCommand && !activeSelectSkipToMode;

        public void ResetDefault()
        {
            HP = DataGlobal.HpDefault;
            Mail = DataGlobal.MailDefault;
            commandCountTime = 0;
            waitCoolDownJump = false;
            playActionCommand = false;
        }

        public bool UpdateHP(int getCountHP)
        {
            HP += getCountHP;
            if (HP <= 0) HP = 0;
            return HP == 0;
        }

        public void UpdateMail(int getCountMail)
        {
            Mail += getCountMail;
            if (Mail > DataGlobal.MailMax) Mail = DataGlobal.MailMax;
            else if (Mail < DataGlobal.MailDefault) Mail = DataGlobal.MailDefault;
        }

        public void UpdateTime(int getCountTime = 1)
        {
            commandCountTime += getCountTime;
        }

        public void AddNewCommand()
        {
            PercentScore -= DataGlobal.minusScoreBoxCommand;
            commandUsedCount++;
        }

        public void RemoveCommand()
        {
            PercentScore += DataGlobal.minusScoreBoxCommand;
            if (PercentScore > 150) PercentScore = 150;
            commandUsedCount--;
        }

        internal void UpdateTime(Transform transform)
        {
            throw new NotImplementedException();
        }
    }
}