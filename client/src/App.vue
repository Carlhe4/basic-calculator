<template>
  <div class="page">
    <div class="halo"></div>
    <main class="card">
      <header>
        <p class="eyebrow">Calculator</p>
        <h1>Basic calculator app</h1>
      </header>

      <form class="calc" @submit.prevent="calculate">
        <label>
          First number
          <input v-model="a" inputmode="decimal" placeholder="12.5" />
        </label>

        <label>
          Operation
          <div class="op-row">
            <button
              v-for="symbol in ops"
              :key="symbol"
              type="button"
              :class="['op-btn', { active: op === symbol }]"
              @click="op = symbol"
            >
              {{ symbol }}
            </button>
          </div>
        </label>

        <label>
          Second number
          <input v-model="b" inputmode="decimal" placeholder="3" />
        </label>

        <button class="primary" type="submit">Calculate</button>
      </form>

      <section class="result" aria-live="polite">
        <p v-if="error" class="error">{{ error }}</p>
        <p v-else-if="result !== null" class="value">Result: {{ result }}</p>
        <p v-else class="hint">Enter two numbers and choose an operation.</p>
      </section>
    </main>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const a = ref('');
const b = ref('');
const op = ref('+');
const result = ref(null);
const error = ref('');
const ops = ['+', '-', '*', '/'];

const calculate = async () => {
  error.value = '';
  result.value = null;

  const first = Number.parseFloat(a.value);
  const second = Number.parseFloat(b.value);

  if (Number.isNaN(first) || Number.isNaN(second))
  {
    error.value = 'Please enter valid numbers.';
    return;
  }

  const response = await fetch('/api/calc', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ a: first, b: second, op: op.value })
  });

  const payload = await response.json();

  if (!response.ok)
  {
    error.value = payload?.error ?? 'Something went wrong.';
    return;
  }

  result.value = payload.result;
};
</script>
