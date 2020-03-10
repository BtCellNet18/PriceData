$(document).ready(function () {

	if (sessionStorage.getItem('data') === null) {
		$.ajax({
			url: 'Data/GetPrices',
			type: 'GET',
			dataType: 'json',
			success: function (data) {
				sessionStorage.setItem('data', JSON.stringify(data))
				reloadTable();
				updateStats();
			}
		});
	} else {
		reloadTable();
		updateStats();
	}
});

function reloadTable() {
	var data = JSON.parse(sessionStorage.getItem('data'));

	$('#prices').DataTable({
		retrieve: true,
		searching: false,
		data: data,
		columns: [
			{ 'data': 'id' },
			{ 'data': 'date' },
			{ 'data': 'value' }
		]
	});
}

function updateStats() {

	var prices = $('#prices').DataTable().column(2).data(); 

	var exp = mostExpensiveHour(prices, 2);

	var prices = prices.sort(function (a, b) { return a - b });

	var min = prices[0];
	var max = prices[prices.length - 1];
	var avg = prices.reduce((a, b) => a + b, 0) / prices.length;

	$('#stats').text(`Min : ${min} Max : ${max} Avg : ${avg} Most expensive hour : ${exp}`);
}

function mostExpensiveHour(nums, k) {
	let result = 0;
	let temp_sum = 0;
	for (var i = 0; i < k - 1; i++) {
		temp_sum += nums[i];
	}
	for (var i = k - 1; i < nums.length; i++) {
		temp_sum += nums[i];
		if (temp_sum > result) {
			result = temp_sum;
		}
		temp_sum -= nums[i - k + 1];
	}
	return result;
}
