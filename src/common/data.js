const labels = new Array(61).fill(0).reduce(
  (prev, item, index) => {
    let splits = prev[index].split(":").map((el) => Number(el));

    splits[1]++;
    if (splits[1] < 10) return [...prev, `${splits[0]}:0${splits[1]}`];
    else if (splits[1] === 60) return [...prev, `${splits[0] + 1}:00`];

    return [...prev, `${splits[0]}:${splits[1]}`];
  },
  ["12:34"]
);

const datasets = [
  {
    label: "ABC",
    data: new Array(61)
      .fill(0)
      .map((el) => Math.round(Math.random() * 100) + el),
    fill: true,
		tension:0.4
  },
  {
    label: "DEF",
    data: new Array(61)
      .fill(0)
      .map((el) => Math.round(Math.random() * 100) + el),
    fill: true,
		tension:0.5
  },
];

export{ labels, datasets}