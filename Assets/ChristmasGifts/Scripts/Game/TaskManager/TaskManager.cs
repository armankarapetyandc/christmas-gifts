using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ChristmasGifts.Scripts.Game.TaskManager
{
    public class TaskManager : MonoBehaviour
    {
        private readonly List<ITask> _tasks = new List<ITask>();
        private bool _isRunning;
        private CancellationTokenSource cts;

        public void Enqueue(ITask task)
        {
            _tasks.Add(task);
        }

        public async void Run(CancellationToken cancellationToken = default)
        {
            if (_isRunning)
            {
                return;
            }

            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            try
            {
                await Process().AttachExternalCancellation(cts.Token);
            }
            catch (TaskCanceledException)
            {
                //ignored
            }
            catch (OperationCanceledException)
            {
                //ignored
            }
        }

        public ITask GetPreviousTask()
        {
            return _tasks.Count==0 ? null : _tasks[^1];
        }

        public void Stop()
        {
            cts.Cancel();
        }

        private async UniTask Process()
        {
            _isRunning = true;
            while (!cts.IsCancellationRequested)
            {
                if (_tasks.Count == 0)
                {
                    await UniTask.Yield(cts.Token);
                    continue;
                }

                ITask task = _tasks[0];
                Debug.Log($"[TaskManager] Executing Task: {task}, Remaining Tasks: {_tasks.Count}");
                await task.Execute().AttachExternalCancellation(cts.Token);
                _tasks.RemoveAt(0);
                Debug.Log($"[TaskManager] Done Task: {task}, Remaining Tasks: {_tasks.Count}");
            }

            _isRunning = false;
        }

        public float EvaluateTasksRemainingTime()
        {
            return _tasks.Sum(task => task.RemainingTime);
        }
    }
}