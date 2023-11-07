<template>
  <LineChart :axes="axes" />
</template>

<script setup lang="ts">
import LineChart from './charts/LineChart.vue'

const axes = {
  x: ["1", "2", "3", "4", "5", "6", "7" ],
  y: [35, 345, 500, 400, 500, 600, 700] 
}
</script>
