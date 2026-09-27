export interface Product {
  id: number;
  description: string;
  salePrice: number;
  category: string;
  image: string;
}

export interface ProductSalesSummary {
  productId: number;
  numberOfSales: number;
  totalQuantity: number;
  totalSales: number;
}