<script setup lang="ts">
import { defineComponent } from 'vue'
import ThePlots from './components/ThePlots.vue'

defineComponent({
  name: 'App',
  components: {
    ThePlots
  }
})
</script>

<template>
  <ThePlots />
</template>

<style scoped></style>
