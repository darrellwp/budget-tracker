// Currency formatters
const axisFormatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    maximumFractionDigits: 0
});

const tooltipFormatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
});

// Elements
const balanceChartElement = document.getElementById('balance-chart');
const categoryLineChartElement = document.getElementById('category-line-chart');
const categoryPieChartElement = document.getElementById('category-pie-chart');

// Chart objects
const balanceChart = echarts.init(balanceChartElement, 'dark');
const categoryLineChart = echarts.init(categoryLineChartElement, 'dark');
const categoryPieChart = echarts.init(categoryPieChartElement, 'dark');

// Chart base options
const balanceChartOptions = {
    backgroundColor: 'transparent',
    color: ['#44db76'],
    tooltip: {
        trigger: 'axis',
        valueFormatter: (value) => tooltipFormatter.format(value)
    },
    grid: {
        top: 10,
        bottom: 10,
        left: 10,
        right: 10
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: [],
        splitLine: {
            show: false,
        },
        axisLine: {
            show: false
        },
        axisTick: {
            show: false
        },
        axisLabel: {
            color: '#44db76'
        },
    },
    yAxis: {
        type: 'value',
        axisTick: {
            show: false
        },
        axisLine: {
            show: false
        },
        axisLabel: {
            color: '#44db76',
            formatter: (value, index) => axisFormatter.format(value)
        }
    },
    series: [
        {
            data: [],
            type: 'line',
            areaStyle: {}
        }
    ]
}

const categoryLineChartOptions = {
    tooltip: {
        trigger: 'axis',
        valueFormatter: (value) => tooltipFormatter.format(value)
    },
    legend: {
        top: 'top',
        orient: 'horizontal',
        data: []
    },
    grid: {
        top: 60,
        left: 10,
        right: 10,
        bottom: 10
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: []
    },
    yAxis: {
        type: 'value',
        min: 0,
        axisLabel: {
            formatter: (value, index) => axisFormatter.format(value)
        }
    },
    series: []
};

const categoryPieChartOptions = {
    tooltip: {
        trigger: 'item'
    },
    legend: {
        top:'top',
        orient: 'horizontal'
    },
    grid: {
        containLabel: true
    },
    series: [
        {
            name: 'Categories',
            type: 'pie',
            radius: ['30%', '65%'],
            center: ['50%', '55%'],
            avoidLabelOverlap: true,
            padAngle: 5,
            itemStyle: {
                borderRadius: 10
            },
            label: {
                show: false
            },
            labelLine: {
                show: false
            },
            data: []
        }
    ]
}

document.addEventListener('DOMContentLoaded', () => {
    getChartData('ytd');

    window.addEventListener('resize', () => {
        balanceChart.resize();
        categoryLineChart.resize();
        categoryPieChart.resize();
    })
});

/**
 * Fetches the data from the server to display the charts
 * @param {any} view
 */
async function getChartData(view) {
    const response = await fetch(`?handler=Dashboard&view=${view}`);

    if (!response.ok) {
        return showToast('Uh-oh! Looks like we had an issue loading your charts. Please try again later.');
    }

    const data = await response.json();

    setBalanceChart(data.balances);
    setCategoryLineChart(data);
    setCategoryPieChart(data);
}

/**
 * Sets the options for the balance chart
 * @param {any} balanceChartData
 */
function setBalanceChart(balanceChartData) {
    const color = balanceChartData[balanceChartData.length - 1].amount > 0 ? '#44db76' : '#db4463';
    const totalSpan = document.getElementById('spanBalanceTotal');

    const options = {
        ...balanceChartOptions,
        color: [color],
        xAxis: {
            ...balanceChartOptions.xAxis,
            axisLabel: {
                color: color
            },
            data: balanceChartData.map(x => x.label)
        },
        yAxis: {
            ...balanceChartOptions.yAxis,
            axisLabel: {
                ...balanceChartOptions.yAxis.axisLabel,
                color: color
            }
        },
        series: [
            {
                ...balanceChartOptions.series[0],
                data: balanceChartData.map(x => x.amount)
            }
        ]
    };

    // Set the total label
    totalSpan.textContent = `$${balanceChartData[balanceChartData.length - 1].amount}`;
    totalSpan.style.color = color;

    balanceChart.setOption(options);
}

/**
 * Sets the options for the line category chart
 * @param {any} balanceChartData
 */
function setCategoryLineChart(categoryLineData) {
    const options = {
        ...categoryLineChartOptions,
        legend: {
            ...categoryLineChartOptions.legend,
            data: categoryLineData.categoryLabels
        },
        xAxis: {
            ...categoryLineChartOptions.xAxis,
            data: categoryLineData.categories.map(x => x.label)
        },
        series: categoryLineData.categoryLabels.map(x => {
            return {
                name: x,
                type: 'line',
                data: categoryLineData.categories.map(y => y.amounts.find(z => z.category == x).amount)
            }
        })
    }

    categoryLineChart.setOption(options);
}

/**
 * Sets the options for the category total pie chart
 * @param {any} balanceChartData
 */
function setCategoryPieChart(categoryPieData) {
    const options = {
        ...categoryPieChartOptions,
        series: [{
            ...categoryPieChartOptions.series[0],
            data: categoryPieData.categoryLabels.map(category => {
                return {
                    name: category,
                    value: categoryPieData.categories.map(x => x.amounts.find(y => y.category == category).amount).reduce((acc, value) => acc + value, 0)
                }
            })
        }]
    }

    categoryPieChart.setOption(options);
}