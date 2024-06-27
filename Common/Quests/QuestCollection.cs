using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Common.Quests
{
    public class QuestCollection : IEnumerable<QuestData>
    {
        private QuestData[] _quests;
        public QuestCollection(params QuestData[] quests)
        {
            _quests = new QuestData[quests.Length];
            for (int i = 0; i < quests.Length; i++)
            {
                _quests[i] = quests[i];
            }
        }
        public void Add(QuestData questToAdd)
        {
            if (_quests.Length >= Array.MaxLength)
            {
                throw new InvalidOperationException();
            }
            else
            {
                QuestData[] newQuests = new QuestData[_quests.Length + 1];
                for (int i = 0; i < _quests.Length; i++)
                {
                    newQuests[i] = _quests[i];
                }
                newQuests[^1] = questToAdd;
                _quests = newQuests;
            }
        }
        public void Add(QuestID questType, int typeRequired, int amountRequired, bool hardmode)
        {
            Add(new QuestData(questType, typeRequired, amountRequired, hardmode));
        }
        public QuestCollection ChainAdd(QuestData questToAdd)
        {
            if (_quests.Length >= Array.MaxLength)
            {
                throw new InvalidOperationException();
            }
            else
            {
                QuestData[] newQuests = new QuestData[_quests.Length + 1];
                for (int i = 0; i < _quests.Length; i++)
                {
                    newQuests[i] = _quests[i];
                }
                newQuests[^1] = questToAdd;
                _quests = newQuests;
                return this;
            }
        }
        public QuestCollection ChainAdd(QuestID questType, int typeRequired, int amountRequired, bool hardmode)
        {
            return ChainAdd(new QuestData(questType, typeRequired, amountRequired, hardmode));
        }

        public IEnumerator<QuestData> GetEnumerator() => new QuestEnumerator(_quests);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Length => _quests.Length;
        public QuestData this[int i] { get => _quests[i]; set => _quests[i] = value; }
    }
    public class QuestEnumerator : IEnumerator<QuestData>
    {
        private QuestData[] _quests;
        int _index = -1;
        public QuestEnumerator(QuestData[] quests)
        {
            _quests = quests;
        }
        public QuestData Current
        {
            get
            {
                try
                {
                    return _quests[_index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            return _index++ < _quests.Length;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
