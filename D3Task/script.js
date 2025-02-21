const margin = { top: 20, right: 60, bottom: 30, left: 40 };
const width = 600 - margin.left - margin.right;
const height = 400 - margin.top - margin.bottom;

const colors = ['#1f77b4', '#ff7f0e', '#2ca02c', '#d62728', '#9467bd'];

const svg = d3.select("#chart")
    .append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom)
    .append("g")
    .attr("transform", `translate(${margin.left},${margin.top})`);

/**
 * @param {string} input
 * @returns {boolean} 
 */
function validateInput(input) {
    if (!input.trim()) return false;
    
    const numbers = input.split(',').map(n => n.trim());
    
    // Verificar que todos sean números enteros
    return numbers.every(n => {
        const num = Number(n);
        return !isNaN(num) && Number.isInteger(num) && num >= 0;
    });
}

/**
 * @param {number[]} data - 
 */
function updateChart(data) {
    svg.selectAll("*").remove();

    const y = d3.scaleBand()
        .range([0, height])
        .padding(0.1);

    const x = d3.scaleLinear()
        .range([0, width]);

    y.domain(data.map((d, i) => i));
    x.domain([0, d3.max(data)]);

    svg.selectAll(".bar")
        .data(data)
        .enter()
        .append("rect")
        .attr("class", "bar")
        .attr("y", (d, i) => y(i))
        .attr("height", y.bandwidth())
        .attr("x", 0)
        .attr("width", d => x(d))
        .attr("fill", (d, i) => colors[i % colors.length]);

    svg.selectAll(".bar-label")
        .data(data)
        .enter()
        .append("text")
        .attr("class", "bar-label")
        .attr("y", (d, i) => y(i) + y.bandwidth() / 2)
        .attr("x", d => x(d) + 5)
        .attr("dy", ".35em")
        .text(d => d);

    svg.append("g")
        .call(d3.axisLeft(y));

    svg.append("g")
        .attr("transform", `translate(0,${height})`)
        .call(d3.axisBottom(x));
}

document.getElementById("updateData").addEventListener("click", () => {
    const input = document.getElementById("sourceData").value;
    const errorDiv = document.getElementById("error");
    
    if (validateInput(input)) {
        errorDiv.style.display = "none";
        const data = input.split(',').map(n => parseInt(n.trim()));
        updateChart(data);
    } else {
        errorDiv.style.display = "block";
    }
});

document.getElementById("sourceData").value = "4,8,15,16";
document.getElementById("updateData").click();