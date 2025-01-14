import { Chart, registerables } from "chart.js";
import { forwardRef, useEffect, useRef } from "react";

/* constants */
import { labels, datasets } from "../common/data";

const tooltipNote = {
  id: "tooltipNote",
  defaults: {
    x0: 0,
    x1: 1,
    xMove: 0,
    isDrag: false,
    isMoveIn: false,
  },
  afterTooltipDraw(chart, args, pluginOptions) {
    console.log("tooltip", pluginOptions.isDrag);
  },
  afterUpdate(chart, args, pluginOptions) {
    const { scales, options } = chart;
    const { isDrag, x0, x1 } = pluginOptions;
    const { mode } = args;
    if (isDrag && mode === "mouseup") {
      let _x0 = scales.x.getValueForPixel(x0),
        _x1 = scales.x.getValueForPixel(x1);

      options.scales.x.min = _x0;
      options.scales.x.max = _x1;
      pluginOptions.isDrag = false;
    }
  },
  afterDatasetsDraw(chart, args, pluginOptions) {
    const {
      ctx,
      chartArea: { top, height },
    } = chart;

    const { x0, x1, isDrag, xMove } = pluginOptions;
    if (isDrag) {
      ctx.beginPath();
      ctx.fillStyle = "rgba(225,225,225,0.5)";
      if (x0 > x1) {
        ctx.fillRect(x1, top, x0 - x1, height);
      } else {
        ctx.fillRect(x0, top, x1 - x0, height);
      }
      ctx.strokeStyle = "rgba(225,225,225,1)";
      ctx.restore();
    }

    if (!!xMove) {
      ctx.fillStyle = "rgba(0,0,0,0.5)";
      ctx.fillRect(xMove, top, 1, height);
    }
  },
  afterEvent(chart, args, pluginOptions) {
    const {
      event: { x },
    } = args;
    const {
      canvas,
      chartArea: { left, right },
    } = chart;

    const { isDrag } = pluginOptions;
    if (x < left || x > right) {
      pluginOptions.xMove = 0;
    } else {
      pluginOptions.xMove = x;
    }

    if (!isDrag) {
      canvas.addEventListener("mousedown", () => {
        pluginOptions.x0 = x;
        pluginOptions.isDrag = true;
      });
    } else {
      pluginOptions.x1 = x;
      canvas.addEventListener("mouseup", () => {
        chart.update("mouseup");
      });
    }
  },
};

Chart.register(...registerables, tooltipNote);

const ChartBar = (_, refCurrent) => {
  const refChart = useRef(null);

  useEffect(() => {
    if (refChart.current) {
      refCurrent.current = new Chart(refChart.current, {
        type: "line",
        data: {
          labels,
          datasets,
        },

        options: {
          responsive: true,
          plugins: {
            legend: {
              position: "bottom",
            },
            tooltip: {
              mode: "index",
              bodyFont: (ctx, args) => {
                console.log(ctx.tooltipItems);
                return {
                  size: 10,
                };
              },
            },
          },
          interaction: {
            axis: "x",
            intersect: false,
          },
          scales: {
            y: {
              beginAtZero: true,
              stacked: true,
            },
          },
        },
      });
      refChart.current.style.height = "calc(100vh - 40px)";
    }
    return () => {
      if (!!refCurrent.current) refCurrent.current.destroy();
    };
  }, [refCurrent]);

  return <canvas ref={refChart}></canvas>;
};

export default forwardRef(ChartBar);
